# SHFB-Plugins
Plugins for Sandcastle Help File Builder (SHFD)
## Prebuild XSLT transformation plug-in
This plug-in allows you to apply XSLT transformation to topic source files before building help file. E.g., it can be used to simlify source xml and make it more readable and/or contextual.
###### Example
You have list of versions, grouped by some category:
```xml
<section>
  <title>1.0.1801.1</title>
  <content>
    <list class="bullet">
      <listItem>
        <para>General</para>
        <list class="bullet">
          <listItem>
            <para>Some general change A.</para>
          </listItem>
          <listItem>
            <para>Some general change B.</para>
          </listItem>
        </list>
      </listItem>
      <listItem>
        <para>Bug fixes</para>
        <list class="bullet">
          <listItem>
            <para>Some bug fix 1.</para>
          </listItem>
          <listItem>
            <para>Some bug fix 2.</para>
          </listItem>
        </list>
      </listItem>
    <list>
  </content>
</section>
<section>
  <title>1.0.1801.2</title>
  <content>
    <list class="bullet">
      <listItem>
        <para>General</para>
        <list class="bullet">
          <listItem>
            <para>Some general change C.</para>
          </listItem>
          <listItem>
            <para>Some general change D.</para>
          </listItem>
        </list>
      </listItem>
      <listItem>
        <para>Bug fixes</para>
        <list class="bullet">
          <listItem>
            <para>Some bug fix 3.</para>
          </listItem>
          <listItem>
            <para>Some bug fix 4.</para>
          </listItem>
        </list>
      </listItem>
    <list>
  </content>
</section>
```
Using XSLT transformation it can be simplified to next:
```xml
<t:version value="1.0.1801.1">
  <t:changesGroup name="General">
    <t:change>Some general change A.</t:change>
    <t:change>Some general change B.</t:change>
  </t:changesGroup name="Bug fixes">
    <t:change>Some bug fix 1.</t:change>
    <t:change>Some bug fix 2.</t:change>
  </t:changesGroup>
</t:version>
<t:version value="1.0.1801.2">
  <t:changesGroup name="General">
    <t:change>Some general change C.</t:change>
    <t:change>Some general change D.</t:change>
  </t:changesGroup name="Bug fixes">
    <t:change>Some bug fix 3.</t:change>
    <t:change>Some bug fix 4.</t:change>
  </t:changesGroup>
</t:version>
```
###### Plug-in usage
To apply XSLT transformation to your topic source xml you need:
- Create file with same name as topic source file, but ".xslt" extension (it's your XSLT transformation file).
- Inclede file in your project in same directory where topic source file located.
- Enable plug-in.

So, it should looks like:
```
─ (Project root)
  └ Content
    ├ TopicA.aml
    ├ TopicB.aml
    ├ TopicB.xslt
    ├ TopicC.aml
    ├ TopicC.xslt
    └ TopicD.aml
```
In this case, to `TopicB.aml` and `TopicC.aml` XSLT transformation will be applied.
