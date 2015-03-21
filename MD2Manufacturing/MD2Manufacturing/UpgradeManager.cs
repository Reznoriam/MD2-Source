using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD2
{
    public class UpgradeManager : Saveable
    {
        private AssemblyLine parent;
        private List<AssemblyLineUpgradeDef> CompletedUpgrades = new List<AssemblyLineUpgradeDef>();
        private List<AssemblyLineUpgradeDef> AvailableUpgrades = new List<AssemblyLineUpgradeDef>();
        private List<AssemblyLineUpgrade> UpgradesInProgress = new List<AssemblyLineUpgrade>();
        private bool firstLoad = true;

        private Vector2 upgradeEntrySize;
        private readonly float margin = 4f;


        public UpgradeManager(AssemblyLine parent)
        {
            this.parent = parent;
        }

        public void SpawnSetup()
        {
            if (firstLoad)
            {
                firstLoad = false;
                AvailableUpgrades = (
                    from t in DefDatabase<AssemblyLineUpgradeDef>.AllDefs.Where((AssemblyLineUpgradeDef def) => !CompletedUpgrades.Contains(def))
                    select t).ToList();
            }
        }

        public void Tick()
        {
            if (UpgradesInProgress.Count > 0)
            {
                List<AssemblyLineUpgrade> list = new List<AssemblyLineUpgrade>();
                foreach (var upgrade in UpgradesInProgress)
                {
                    if (upgrade.Tick())
                    {
                        parent.AddUpgrade(upgrade);
                        list.Add(upgrade);
                    }
                }
                if (list.Count > 0)
                {
                    foreach (var u in list)
                    {
                        UpgradesInProgress.Remove(u);
                        CompletedUpgrades.Add(u.Def);
                    }
                }
            }
        }


        public AssemblyLine Parent
        {
            get
            {
                return this.parent;
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.LookList(ref this.CompletedUpgrades, "completedUpgrades", LookMode.DefReference);
            Scribe_Collections.LookList(ref this.UpgradesInProgress, "upgradesinProgress", LookMode.Deep);
        }

        public void OnGUI(Rect inRect, Vector2 scrollPosition)
        {
            upgradeEntrySize = new Vector2(inRect.width - (margin * 6), 70f);

            //Widgets.DrawMenuSection(inRect);
            List<AssemblyLineUpgradeDef> availableList = (
                from t in AvailableUpgrades
                where t.alwaysDisplay || !CompletedUpgrades.Contains(t) && (t.Prerequisites == null || CompletedUpgrades.ContainsAll(t.Prerequisites))
                orderby t.label
                select t).ToList();

            if (availableList.Count <= 0)
            {
                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(inRect, "NoAvailableUpgrades".Translate());
                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.UpperLeft;
            }
            else
            {
                try
                {
                    GUI.BeginGroup(inRect);
                    float entryHeightWithMargin = upgradeEntrySize.y + 8f;
                    float currentScrollY = 3f;
                    float height = (float)availableList.Count * entryHeightWithMargin+currentScrollY;

                    Rect viewRect = new Rect(0, 0, upgradeEntrySize.x, height);
                    Rect outRect = new Rect(inRect.AtZero());
                    try
                    {
                        scrollPosition = Widgets.BeginScrollView(outRect, scrollPosition, viewRect);

                        foreach (var current in availableList)
                        {
                            DrawUpgradeEntry(current, currentScrollY);
                            currentScrollY += entryHeightWithMargin;
                        }
                    }
                    finally
                    {
                        Widgets.EndScrollView();
                    }
                }
                finally
                {
                    GUI.EndGroup();
                }
            }
        }

        private void DrawUpgradeEntry(AssemblyLineUpgradeDef def, float currentScrollY)
        {
            Rect currentEntryRect = new Rect(margin, currentScrollY, upgradeEntrySize.x, upgradeEntrySize.y);
            Widgets.DrawMenuSection(currentEntryRect);
            Rect textureRect = currentEntryRect.ContractedBy(2f);
            Rect innerRect = currentEntryRect.ContractedBy(6f);
            Vector2 butSize = new Vector2(60f, innerRect.height / 2);
            float padding = 2f;

            try
            {
                GUI.BeginGroup(innerRect);

                Rect textBox = new Rect(0, 0, innerRect.width / 2, innerRect.height);
                Widgets.Label(textBox, def.label);

                Rect butRect = new Rect(innerRect.width - (butSize.x + padding), innerRect.height / 2 - (butSize.y / 2), butSize.x, butSize.y);
                if (Widgets.TextButton(butRect, "UpgradeInstall".Translate()))
                {
                    CompletedUpgrades.Add(def);
                }
            }
            finally
            {
                GUI.EndGroup();
            }
        }

    }
}
