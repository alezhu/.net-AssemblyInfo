<?xml version="1.0" encoding="Windows-1251" standalone="yes" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
xmlns:xsls="http://www.w3.org/TR/WD-xsl">
<xsl:output method="html"/>

<xsl:template match="/">
<html>
<head>
<style>
h2, h3 {
color: blue;
}
div {
margin: 0px 0px 0px 0px;
padding: 4px 4px 4px 4px; 

}
body, table {
font-family: Arial, Helvetica, sans-serif;
font-size:10pt;
font-weight:normal;
background-color:white
}

a {
	color:green;
}

table, td, th {
	border-collapse:collapse;
	border: silver solid 1px;
	padding: 5px;
}

.TypeMember {
	font-weight:bold;
	color:blue;
}

.ReservedWord {
	color: blue;
}

.rowodd {background-color: #E9E9E9; color: black}
.roweven {background-color: #F8F8F8; color: black}

span.topnavigation {
	margin-left: 5pt;
	margin-right:5pt;
	font-weight:bold;
	display:none;
}

span.topnavigation a{
	margin-left: 1pt;
	margin-right:1pt;
}

span.topnavigation a:hover {
	background-color:green;
	color:white;
}

span.TypeMember a {
	color:Blue;
}

tr.SpecialName {
	display:none;
}

</style>
<title><xsl:value-of select="/type/fullname"/></title>
<script language="JavaScript">
<xsl:comment>
<![CDATA[
	function ShowLink(id) {
		var a = document.getElementById(id);
		if (a) {
			a.style.display = "inline";
		}
	}

	var bShowSpecialName = false;
	function ShowSpecialName() {
		bShowSpecialName = !bShowSpecialName ;
		var tags = document.all.tags("TR");
		for(i=0;i<tags.length;i++) {
			if (tags[i].style.display == "none") tags[i].style.display = (bShowSpecialName )?"inline":"none";
		}
		return false;
	}
]]>
</xsl:comment>
</script>

</head>
<body>
	<a name="#top" />
	<xsl:apply-templates />
</body>
</html>
</xsl:template>

<xsl:template match="type">
<h1><xsl:value-of select="/type/fullname"/></h1>
	<table>
	<tr><td>Namespace:</td><td><xsl:value-of select="namespace"/></td></tr>
	<tr><td>Assembly:</td><td><xsl:value-of select="assembly"/></td></tr>
	<tr><td>Base Class:</td><td><xsl:apply-templates select="base"/></td></tr>
	</table>
	<div>
		<input type="submit" OnClick = "ShowSpecialName();" value="Показывать скрытые поля и методы" />
		<span id="constructors" class = "topnavigation">[<a href="#constructors">Constructors</a>]</span>
		<span id="properties" class = "topnavigation">[<a class = "topnavigation" href="#properties">Properties</a>]</span>
		<span id="fields" class = "topnavigation">[<a class = "topnavigation" href="#fields">Fields</a>]</span>
		<span id="methods" class = "topnavigation">[<a class = "topnavigation" href="#methods">Methods</a>]</span>
		<span id="events" class = "topnavigation">[<a class = "topnavigation" href="#events">Events</a>]</span>
		<span id="interfaces" class = "topnavigation">[<a class = "topnavigation" href="#interfaces">Interfaces</a>]</span>
	</div>
	<xsl:apply-templates select="properties|fields|methods|events|interfaces|constructors"/>
</xsl:template>

<xsl:template match="base">
	<xsl:choose>
		<xsl:when test="@href"><a href="{@href}.xml"><xsl:apply-templates /></a></xsl:when>
		<xsl:otherwise><xsl:apply-templates /></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="properties|fields|methods|events|interfaces|constructors">
	<div><span class="TypeMember"><a name="#{name()}"><b><xsl:value-of select="name()"/></b></a></span><span class="topnavigation" style="display:inline" ><a href="#top">^вверх^</a></span>
<script language="JavaScript">
	var id = "<xsl:value-of select="name()"/>" ;
<xsl:comment>
<![CDATA[
ShowLink(id);
]]>
</xsl:comment>
</script>

	<table>
	<xsl:choose>
		<xsl:when test="name() = 'properties'"><xsl:call-template name="properties_header"/></xsl:when>
		<xsl:when test="name() = 'fields'"><xsl:call-template name="fields_header"/></xsl:when>
		<xsl:when test="name() = 'methods'"><xsl:call-template name="methods_header"/></xsl:when>
		<xsl:when test="name() = 'constructors'"><xsl:call-template name="constructors_header"/></xsl:when>
		<xsl:when test="name() = 'events'"><xsl:call-template name="events_header"/></xsl:when>
		<xsl:when test="name() = 'interfaces'"><xsl:call-template name="interfaces_header"/></xsl:when>
	</xsl:choose>
	
	<xsl:for-each select="item">
		<xsl:sort select="@name"/>
		<tr>
			<xsl:attribute name="class">
				<xsl:choose>
					<xsl:when test="position() mod 2 = 1">rowodd</xsl:when>
					<xsl:when test="position() mod 2 = 0">roweven</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:choose>
					<xsl:when test="(./isspecialname/. = '1') and (not (@name = '.ctor')) and (not (@name = '.cctor'))">display:none</xsl:when>
					<xsl:otherwise></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:apply-templates select="."/>
		</tr>
	</xsl:for-each>
	</table>
	</div>
</xsl:template>

<xsl:template match="property|field|method|event|interface|item">
	<td><b><xsl:value-of select="@name"/></b></td><td><xsl:value-of select="definition"/></td>
</xsl:template>

<xsl:template name="properties_header">
	<th><b>Name</b></th><th>Type</th>
	<th title="CanRead" alt="CanRead">R</th>
	<th title="CanWrite" alt="CanWrite">W</th>
	<th title="IsSpecialName" alt="IsSpecialName">SN</th>
	<th>GetMethod</th><th>SetMethod</th>
	<th>Attributes</th>
</xsl:template>

<xsl:template match="properties/item">
	<td><b><a name="#{@name}" /><xsl:value-of select="@name"/></b></td>
	<td><xsl:apply-templates select="propertytype"/></td>
	<td><xsl:apply-templates select="canread"/></td>
	<td><xsl:apply-templates select="canwrite"/></td>
	<td><xsl:apply-templates select="isspecialname"/></td>
	<td><xsl:apply-templates select="getmethod"/></td>
	<td><xsl:apply-templates select="setmethod"/></td>
	<td><xsl:apply-templates select="customattributes"/></td>
</xsl:template>

<xsl:template match="propertytype">
	<xsl:choose>
		<xsl:when test="@href"><a href="{@href}.xml"><xsl:apply-templates /></a></xsl:when>
		<xsl:otherwise><xsl:apply-templates /></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="canread|canwrite|isspecialname">
	<xsl:choose>
		<xsl:when test=". = '1'">+</xsl:when>
		<xsl:otherwise>-</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="getmethod|setmethod">
	<xsl:choose>
		<xsl:when test=". = ''">-</xsl:when>
		<xsl:otherwise><xsl:apply-templates /></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="customattributes">
	<div>
	<xsl:for-each select="item">
		<xsl:if test="position() > 1">, </xsl:if>
		<a href="{@href}.xml">[<xsl:apply-templates />]</a>
	</xsl:for-each>
	</div>

</xsl:template>


<xsl:template name="fields_header">
	<th><b>Name</b></th><th>Definition</th><th>Attributes</th>
</xsl:template>

<xsl:template match="fields/item">
	<td><b><a name="#{@name}" /><xsl:value-of select="@name"/></b></td>
	<td><i class="ReservedWord">
		<xsl:if test="isstatic/. = '1'">Static </xsl:if>
		<xsl:if test="isassembly/. = '1'">Assembly </xsl:if>
		<xsl:if test="isfamily/. = '1'">Family </xsl:if>
		<xsl:if test="isfamilyandassembly/. = '1'">FamilyAndAssembly </xsl:if>
		<xsl:if test="isfamilyorassembly/. = '1'">FamilyOrAssembly </xsl:if>
		<xsl:if test="isinitonly/. = '1'">InitOnly </xsl:if>
		<xsl:if test="isliteral/. = '1'">Literal </xsl:if>
		<xsl:if test="isnotserialized/. = '1'">NotSerialized </xsl:if>
		<xsl:if test="ispinvokeimpl/. = '1'">PinvokeImpl </xsl:if>
		<xsl:if test="isprivate/. = '1'">Private </xsl:if>
		<xsl:if test="ispublic/. = '1'">Public </xsl:if>
		<xsl:if test="isspecialname/. = '1'">SpecialName </xsl:if>
		</i>
		<xsl:apply-templates select="fieldtype"/><xsl:text> </xsl:text><xsl:value-of select="@name"/>;
	</td>
	<td><xsl:apply-templates select="customattributes"/></td>
</xsl:template>

<xsl:template match="fieldtype|returntype|parametertype|eventhandlertype|interfacetype">
	<xsl:choose>
		<xsl:when test="@href"><a href="{@href}.xml"><xsl:apply-templates /></a></xsl:when>
		<xsl:otherwise><xsl:apply-templates /></xsl:otherwise>
	</xsl:choose>
</xsl:template>



<xsl:template name="methods_header">
	<th><b>Name</b></th><th>Definition</th><th>Attributes</th>
</xsl:template>

<xsl:template match="methods/item">
	<td><b><a name="#{@name}" /><xsl:value-of select="@name"/></b></td>
	<td>
		<xsl:call-template name="method"/>
	</td>
	<td><xsl:apply-templates select="customattributes"/></td>
</xsl:template>

<xsl:template name="method">
	<i class="ReservedWord">
		<xsl:if test="isabstract/. = '1'">Abstract </xsl:if>
		<xsl:if test="isassembly/. = '1'">Assembly </xsl:if>
		<xsl:if test="isconstructor/. = '1'">Constructor </xsl:if>
		<xsl:if test="isfamily/. = '1'">Family </xsl:if>
		<xsl:if test="isfamilyandassembly/. = '1'">FamilyAndAssembly </xsl:if>
		<xsl:if test="isfamilyorassembly/. = '1'">FamilyOrAssembly </xsl:if>
		<xsl:if test="isfinal/. = '1'">Final </xsl:if>
		<xsl:if test="isprivate/. = '1'">Private </xsl:if>
		<xsl:if test="ispublic/. = '1'">Public </xsl:if>
		<xsl:if test="isspecialname/. = '1'">SpecialName </xsl:if>
		<xsl:if test="isstatic/. = '1'">Static </xsl:if>
		<xsl:if test="isvirtual/. = '1'">Virtual </xsl:if>
		</i>
		<xsl:apply-templates select="returntypeattributes/customattributes"/><xsl:text> </xsl:text>
		<xsl:apply-templates select="returntype"/><xsl:text> </xsl:text><xsl:value-of select="@name"/>
		<xsl:text> (</xsl:text><xsl:apply-templates select="parameters"/>);
</xsl:template>

<xsl:template name="constructors_header">
	<a name="#constructors" />
	<th>Definition</th><th>Attributes</th>
</xsl:template>

<xsl:template match="constructors/item">
	<td><i class="ReservedWord">
		<xsl:if test="isabstract/. = '1'">Abstract </xsl:if>
		<xsl:if test="isassembly/. = '1'">Assembly </xsl:if>
		<xsl:if test="isfamily/. = '1'">Family </xsl:if>
		<xsl:if test="isfamilyandassembly/. = '1'">FamilyAndAssembly </xsl:if>
		<xsl:if test="isfamilyorassembly/. = '1'">FamilyOrAssembly </xsl:if>
		<xsl:if test="isfinal/. = '1'">Final </xsl:if>
		<xsl:if test="isprivate/. = '1'">Private </xsl:if>
		<xsl:if test="ispublic/. = '1'">Public </xsl:if>
		<xsl:if test="isstatic/. = '1'">Static </xsl:if>
		<xsl:if test="isvirtual/. = '1'">Virtual </xsl:if>
		</i>
		<b><a name="#{@name}" /><xsl:value-of select="declaringtype/."/></b>
		<xsl:text> (</xsl:text><xsl:apply-templates select="parameters"/>);

	</td>
	<td><xsl:apply-templates select="customattributes"/></td>
</xsl:template>


<xsl:template match="parameters">
	<xsl:for-each select="item"><xsl:sort select="./position"/>
		<xsl:if test="position() > 1">, </xsl:if><xsl:apply-templates select="."/>
	</xsl:for-each>
</xsl:template>

<xsl:template match="parameters/item">
	<i class="ReservedWord">
		<xsl:if test="islcid/. = '1'">LCID </xsl:if>
		<xsl:if test="isin/. = '1'">In </xsl:if>
		<xsl:if test="isout/. = '1'">Out </xsl:if>
		<xsl:if test="isretval/. = '1'">RetVal </xsl:if>
		<xsl:if test="isoptional/. = '1'">Optional </xsl:if>
		</i>
		<xsl:apply-templates select="parametertype"/><xsl:text> </xsl:text><xsl:value-of select="@name"/>
		<xsl:if test="not(defaultvalue ='')"> = <xsl:value-of select="defaultvalue"/></xsl:if>
</xsl:template>

<xsl:template name="events_header">
	<th><b>Name</b></th><th>Definition</th><th>Attributes</th>
</xsl:template>

<xsl:template match="events/item">
	<td><b><a name="#{@name}" /><xsl:value-of select="@name"/></b></td>
	<td><i class="ReservedWord">
		<xsl:if test="ismulticast/. = '1'">Multicast </xsl:if>
		<xsl:if test="isspecialname/. = '1'">SpecialName </xsl:if>
		</i>
		<xsl:apply-templates select="eventhandlertype"/><xsl:text> </xsl:text><xsl:value-of select="@name"/>
	</td>
	<td><xsl:apply-templates select="customattributes"/></td>
</xsl:template>

<xsl:template name="interfaces_header">
	<th><b>Name</b></th><th>Definition</th><th>Attributes</th>
</xsl:template>

<xsl:template match="interfaces/item">
	<td><b><a name="#{@name}" /><xsl:value-of select="@name"/></b></td>
	<td><xsl:apply-templates select="interfacetype"/><xsl:text> </xsl:text><xsl:value-of select="@name"/>
	</td>
	<td><xsl:apply-templates select="customattributes"/></td>
</xsl:template>


</xsl:stylesheet>

