<project name="AHK.ScriptsMan" guid="{41a40866-f247-428b-a6de-ab5e70d21da9}" notes="this is the parent project of ChameleonCoder. It is deprecated now." description="[deprecated] IDE" language="{f67fb9f3-ea9c-4140-b960-f47a44b5de56}" priority="2">
  <link name="resource dll" guid="{a3d7f867-16d7-4260-803a-74a58aeba668}" destination="{d315d152-7e7f-4028-a49a-b5c95c66762c}" />
  <link name="executable" guid="{327a7eb0-07ad-4219-a4b4-878920075fc5}" destination="{5d03da7b-acf1-4ae9-acca-6a4133c65880}" />
  <metadata name="Aktuell" noconfig="1" default="0">Neusortierung</metadata>
  <metadata name="MainElement" noconfig="0" default="1">Treeview</metadata>
  <task name="Code review" guid="{398aca26-d030-4b19-9a45-16edef1d70ca}" notes="code should be reviewed and shortened, especially Gui.ahk.&#xD;&#xA;Maybe I should use new class syntax... ?" description="replace old code with new one" enddate="20110912" />
  <group name="UI" guid="{0c49bd9e-939c-4afc-9611-a9dc644eacea}" notes="this is the most unfinished part&#xD;&#xA;It requires Scintilla class by HotkeyIt --&gt; download" description="contains the gui code" language="{82ff6b49-18fd-42ba-bd1e-e150af60e0a5}" priority="2">
    <project name="PanelMain" guid="{fcf88c90-997a-4498-958c-695dbbd20f24}" notes="- rework code&#xD;&#xA;- save documents in object / array&#xD;&#xA;- save panel handles" description="the main gui code file" language="{82ff6b49-18fd-42ba-bd1e-e150af60e0a5}" priority="2">
      <link name="toolbar" guid="{bc38b097-3766-4121-8977-a8d9ca99317f}" destination="{5d03da7b-acf1-4ae9-acca-6a4133c65880}" />
      <task name="Continue" guid="{789804aa-07f2-4472-bb65-d97009b6cfb2}" notes="Continue working out the main panel design&#xD;&#xA;	- first make notes&#xD;&#xA;	- then draw&#xD;&#xA;	- then code ;)" description="Finish other panels &amp; events">
        <code name="AutomaticGuiCreator.ahk" guid="{46f73fd9-a112-4bb2-ab27-5d12b717dc3d}" notes="this script is incredible! It creates code from drawings and optimizes them!" description="auto-creates and reworks GUI code" language="{82ff6b49-18fd-42ba-bd1e-e150af60e0a5}" compilation-path="" path="C:\anywhere\AGC.ahk" />
      </task>
    </project>
  </group>
  <library name="SVS-Funktionen" guid="{9dcb55f1-9fed-43bf-98b7-da8b9a1700a1}" notes="" description="divers functions" language="{82ff6b49-18fd-42ba-bd1e-e150af60e0a5}" priority="2">
    <metadata name="Todo" noconfig="0" default="1">Cleanup!</metadata>
  </library>
</project>