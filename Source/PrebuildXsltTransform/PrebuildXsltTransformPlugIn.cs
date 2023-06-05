using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using SandcastleBuilder.Utils;
using SandcastleBuilder.Utils.BuildComponent;
using SandcastleBuilder.Utils.BuildEngine;
using SandcastleBuilder.Utils.ConceptualContent;

namespace PrebuildXsltTransform
{
    [HelpFileBuilderPlugInExport("Prebuild XSLT transformation", Description = "Prebuild XSLT transformation plug-in")]
    public sealed class PrebuildXsltTransformPlugIn : IPlugIn
    {
        private readonly Lazy<List<ExecutionPoint>> executionPoints;
        private BuildProcess builder;

        public PrebuildXsltTransformPlugIn()
        {
            executionPoints = new Lazy<List<ExecutionPoint>>(() => new List<ExecutionPoint>
            {
                new ExecutionPoint(BuildStep.CopyConceptualContent, ExecutionBehaviors.After)
            });
        }

        public IEnumerable<ExecutionPoint> ExecutionPoints => executionPoints.Value;

        public string ConfigurePlugIn(SandcastleProject project, string currentConfig)
            => currentConfig;

        public void Initialize(BuildProcess buildProcess, XElement configuration)
        {
            builder = buildProcess;

            var metadata = (HelpFileBuilderPlugInExportAttribute)GetType()
                .GetCustomAttributes(typeof(HelpFileBuilderPlugInExportAttribute), false)
                .First();

            builder.ReportProgress(metadata.Id);
        }

        public void Execute(ExecutionContext context)
        {
            builder.ReportProgress("-------------------------------");
            builder.ReportProgress("Applying XSLT transforms");
            var targetFolder = Path.Combine(builder.WorkingFolder, "ddueXml");
            foreach (var topic in builder.ConceptualContent.Topics.SelectMany(_ => _))
                TransformTopic(topic, targetFolder);
        }

        private void TransformTopic(Topic topic, string targetFolder)
        {
            if (!topic.NoTopicFile)
            {
                var transformFile = Path.ChangeExtension(topic.TopicFile.FullPath, ".xslt");

                if (builder.CurrentProject.FileItems.Any(_ => _.FullPath == transformFile) && File.Exists(transformFile))
                {
                    builder.ReportProgress($"    Transforms topic {topic.Id} ({topic.TopicFile.FullPath})");
                    var transform = new XslCompiledTransform();
                    transform.Load(transformFile);
                    var topicXmlFileName = Path.Combine(targetFolder, topic.Id + ".xml");

                    using (var buffer = new MemoryStream())
                    using (var writer = XmlWriter.Create(buffer))
                    {
                        transform.Transform(topicXmlFileName, writer);
                        File.WriteAllBytes(topicXmlFileName, buffer.ToArray());
                    }
                }
            }

            foreach (var subtopic in topic.Subtopics)
                TransformTopic(subtopic, targetFolder);
        }

        void IDisposable.Dispose()
        {
        }
    }
}
