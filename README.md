# PublicFolderDumper

A plugin for [NeosVR](https://neos.com) to dump the contents of "The Directory" within Neos to a CSV file. I needed this for Directory management and ease of access. You can find the [completed list on the Neos Wiki](https://wiki.neos.com/List_of_Public_Folders). It also provided a motivation factor to finally get a plugin workflow and environment setup. This area is one I want to tutorialize later.

As far as Plugins for Neos goes, this is very small. While I am familiar with C#, this is my first foray into writing Plugins for Neos. I am very thankful to [Epsilion for their Open Source plugin](https://github.com/Aerizeon/NeosWikiAssetGenerator) of which this project uses many base files and configuration settings.

This plugin has a number of issues:

1. You currently need to copy across the Two Dependencies required. This isn't ideal but it is what it is. If I could get .netstandard 2.1 to work, you'd be down to just one. If you know how to improve this let me know.:
   1. CsvHelper.dll
   2. Microsoft.Bcl.HashCode.dll
2. It assumes you have a D:, I have many hard drives and re-named my main "Data" drive to D:, which is usually a CD. I'll fix this when I'm not tired.

Pull Requests and Comments are welcome, please contact me!, Oh and the code is fully commented to help boost education and knowledge.
