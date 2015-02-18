# Install.ps1
param($installPath, $toolsPath, $package, $project)

$projectPath = Split-Path -Parent $project.FullName

# ********************************** Amend Web.config

$xml = New-Object xml

# find its path on the file system
$localPath = Join-Path $projectPath "Web.config"

# load Web.config as XML
$xml.Load($localPath)

# select the node
$modules = $xml.SelectSingleNode("configuration/system.webServer/modules")
$handlers = $xml.SelectSingleNode("configuration/system.webServer/handlers")

# change the connectionString value
$modules.RemoveAttribute("runAllManagedModulesForAllRequests")

$robots = $xml.CreateElement("add")
$robots.SetAttribute("name", "Robots-ISAPI-Integrated-4.0")
$robots.SetAttribute("path", "/robots.txt")
$robots.SetAttribute("verb", "GET")
$robots.SetAttribute("type", "System.Web.Handlers.TransferRequestHandler")
$robots.SetAttribute("preCondition", "integratedMode,runtimeVersionv4.0")

$sitemap = $xml.CreateElement("add")
$sitemap.SetAttribute("name", "Sitemap-ISAPI-Integrated-4.0")
$sitemap.SetAttribute("path", "/sitemap.xml")
$sitemap.SetAttribute("verb", "GET")
$sitemap.SetAttribute("type", "System.Web.Handlers.TransferRequestHandler")
$sitemap.SetAttribute("preCondition", "integratedMode,runtimeVersionv4.0")

$handlers.AppendChild($robots)
$handlers.AppendChild($sitemap)

$configSections = $xml.SelectSingleNode("configuration/configSections")

$section = $xml.CreateElement("section")
$section.SetAttribute("name", "cluster")
$section.SetAttribute("type", "Yomego.Loyalty.Core.Config.Sections.Servers.ServerSection, Yomego.Loyalty.Core, Version=1.0.0.1, Culture=neutral")
$section.SetAttribute("allowLocation", "true")
$section.SetAttribute("allowDefinition", "Everywhere")

$configSections.AppendChild($section)

$configuration = $xml.SelectSingleNode("configuration")

$cluster = $xml.CreateElement("cluster")
$servers = $xml.CreateElement("servers")
$server = $xml.CreateElement("add")
$server.SetAttribute("Key", "[Your remote site cache identifier]")
$server.SetAttribute("Url", "[Your remote site cache clear url]")

$servers.AppendChild($server)
$cluster.AppendChild($servers)

$configuration.AppendChild($cluster)

# save the Web.config file
$xml.Save($localPath)


# ********************************* Amend ExamineIndex.config

$xmlEi = New-Object xml

# find its path on the file system
$localPathEi = Join-Path $projectPath "config\ExamineIndex.config"

# load Web.config as XML
$xmlEi.Load($localPathEi)

# select the node
$indexSets = $xmlEi.SelectSingleNode("ExamineLuceneIndexSets")

$indexSet = $xmlEi.CreateElement("IndexSet")
$indexSet.SetAttribute("SetName", "WebsiteIndexSet")
$indexSet.SetAttribute("IndexPath", "~/App_Data/TEMP/ExamineIndexes/WebsiteIndex/")

$indexAttribute = $xmlEi.CreateElement("IndexAttributeFields")

$attributeId = $xmlEi.CreateElement("add")
$attributeId.SetAttribute("Name", "id")
$attributeNodeName = $xmlEi.CreateElement("add")
$attributeNodeName.SetAttribute("Name", "nodeName")
$attributeNodeTypeAlias = $xmlEi.CreateElement("add")
$attributeNodeTypeAlias.SetAttribute("Name", "nodeTypeAlias")
$attributeSystemSortOrder = $xmlEi.CreateElement("add")
$attributeSystemSortOrder.SetAttribute("Name", "SystemSortOrder")
$attributeSystemSortOrder.SetAttribute("EnableSorting", "true")
$attributeSystemPublishDate = $xmlEi.CreateElement("add")
$attributeSystemPublishDate.SetAttribute("Name", "SystemPublishDate")
$attributeSystemPublishDate.SetAttribute("EnableSorting", "true")
$attributeContentDatePublished = $xmlEi.CreateElement("add")
$attributeContentDatePublished.SetAttribute("Name", "ContentDatePublished")
$attributeContentDatePublished.SetAttribute("EnableSorting", "true")

$indexAttribute.AppendChild($attributeId)
$indexAttribute.AppendChild($attributeNodeName)
$indexAttribute.AppendChild($attributeNodeTypeAlias)
$indexAttribute.AppendChild($attributeSystemSortOrder)
$indexAttribute.AppendChild($attributeSystemPublishDate)
$indexAttribute.AppendChild($attributeContentDatePublished)

$indexSet.AppendChild($indexAttribute)
$indexSets.AppendChild($indexSet)

# save the Web.config file
$xmlEi.Save($localPathEi)

# ********************************* Amend ExamineSettings.config

$xmlEs = New-Object xml

# find its path on the file system
$localPathEs  = Join-Path $projectPath "config\ExamineSettings.config"

# load Web.config as XML
$xmlEs.Load($localPathEs)

# select the node

$indexProviders = $xmlEs.SelectSingleNode("Examine/ExamineIndexProviders/providers")

$websiteIndexer = $xmlEs.CreateElement("add")
$websiteIndexer.SetAttribute("name", "WebsiteIndexer")
$websiteIndexer.SetAttribute("type", "UmbracoExamine.UmbracoContentIndexer, UmbracoExamine")
$websiteIndexer.SetAttribute("supportUnpublished", "false")
$websiteIndexer.SetAttribute("supportProtected", "true")
$websiteIndexer.SetAttribute("interval", "10")
$websiteIndexer.SetAttribute("analyzer", "Lucene.Net.Analysis.Standard.StandardAnalyzer, Lucene.Net")

$indexProviders.AppendChild($websiteIndexer)

$searchProviders = $xmlEs.SelectSingleNode("Examine/ExamineSearchProviders/providers")

$websiteSearcher = $xmlEs.CreateElement("add")
$websiteSearcher.SetAttribute("name", "WebsiteSearcher")
$websiteSearcher.SetAttribute("type", "UmbracoExamine.UmbracoExamineSearcher, UmbracoExamine")
$websiteSearcher.SetAttribute("analyzer", "Lucene.Net.Analysis.WhitespaceAnalyzer, Lucene.Net")
$websiteSearcher.SetAttribute("enableLeadingWildcards", "true")

$searchProviders.AppendChild($websiteSearcher)


# save the Web.config file
$xmlEs.Save($localPathEs)

# ********************************* Amend en.xml

$xmlEn = New-Object xml

# find its path on the file system
$localPathEn = Join-Path $projectPath "Umbraco\Config\Lang\en.xml"

# load Web.config as XML
$xmlEn.Load($localPathEn)

# select the node

$sections = $xmlEn.SelectSingleNode("language/area[@alias='sections']")

$key = $xmlEn.CreateElement("key")
$key.SetAttribute("alias", "Yomego")
$key.InnerText = "Yomego"

$sections.AppendChild($key)

# save the Web.config file
$xmlEn.Save($localPathEn)

