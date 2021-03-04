using BaseX;
using FrooxEngine;
using FrooxEngine.UIX;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using System;

namespace PublicFolderDumper
{
    [Category("ProbablePrime")]
    public class PublicFolderDumper : Component, ICustomInspector
    {
        // This adds a Slot field to this components, Inspector UI, similar to any other slot field you've seen.
        public readonly SyncRef<Slot> root;

        // We've registered this component as an ICustomInspector, this means we can add stuff to it's inspector UI, Like the "Split"/"Merge" buttons on MeshRenderer.
        public void BuildInspectorUI(UIBuilder ui)
        {
            //Build the standard UI
            WorkerInspector.BuildInspectorUI(this, ui);
            /** Add a button, called "Pull Links", it uses a bunch of wizardry to run the PullLinks method as a task.
             * But I'm not sure if I'm doing this correctly.
             */
            ui.Button("Pull Links", (b, e) => { StartTask(async () => { await PullLinks(); }); });
        }
        /**
         * This main method, will pull all of the links. It loops downwards through a hierarchy finding InventoryLinks and converts them to CSV which it then writes to my computer.
         */
        private async Task PullLinks()
        {
            // Syncrefs are a little weird, but this is due to network sync and stuff.
            Slot slot = this.root.Target;

            // Here we make a list of InventoryLink components. We use a Pool because making a list can be expensive.
            List<InventoryLink> list = Pool.BorrowList<InventoryLink>();

            // This descends down the children hierarchy to find InventoryLinks and puts them in the list.
            slot.GetComponentsInChildren<InventoryLink>(list);

            // We now convert the entire list to a list of CSVItemLinks, this just gets rid of the complexities of it being a "Component"
            List<CSVItemLink> csvList = list.ConvertAll(x => new CSVItemLink { Name = x.TargetName, URL = x.Target.ToString() });

            // Avoids unnecessary errors by, ensuring our target directory exists. This won't work if you don't have a D:.
            if (!Directory.Exists("D:\\NeosWiki\\") 
            {
                Directory.CreateDirectory("D:\\NeosWiki\\")
            }

            /**
             * These are pretty much from the CSVHelper's Docs(https://joshclose.github.io/CsvHelper/)
             * Why are we using a CSV Helper Lib?, Because CSV is really hard sometimes and because we're dumping tons of user generated content here, I don't want to mess it up.
             * Honestly, it would probably be easier to use something manual with string builder, but this scales better for the day when " or , is in a folder name.
             * I'm bias because I have seen a CSV Parser/Writer blow up. I was at a major event for a client and had to panic patch it. So where I can i'll do it "Properly"
             */
            using (var writer = new StreamWriter("D:\\NeosWiki\\Folders.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                csv.WriteRecords(csvList);
                csv.Flush();
            }
            // Return the list to the Pool, because that's the nice thing to do.
            Pool.Return<InventoryLink>(ref list);
        }
    }
    /**
     * This is just a small wrapper class, which allows CsvHelper to write our Records easily.
     * Without this we'd have to do things manually in a big loop of some kind and that didn't seem good.
     * It should probably be in another file, but realistically its only used here and this is a single component plugin.
     * 
     * end result, is name,url in the csv file e.g. Public - ProbablePrime,neosrec:///U-ProbablePrime/R-0776eec6-e4d1-46b0-9996-62428b1310b0
     */
    class CSVItemLink
    {
        // Set the Name to be the first field in the csv output
        [Index(0)]
        public string Name { get; set; }
        // Set the URL to be the second field in the csv output
        [Index(1)]
        public string URL { get; set; }
    }
}
