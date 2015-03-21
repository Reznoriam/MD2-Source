using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD2
{
    public class AssemblyLine : Saveable
    {
        public AssemblyLineProperty Speed;
        public AssemblyLineProperty Efficiency;
        private UpgradeManager upgradeManager;
        public static readonly SettingsDef Settings = DefDatabase<SettingsDef>.GetNamed("MD2AssemblyLineSettings");
        private OrderStack orderStack;
        public string label;

        //Construction variables
        BillOfMaterials billOfMaterials;
        private bool underConstruction = true;
        private bool constructionPaused = false;
        private int constructionTicks = ConstructionTicksRequired;

        public static AssemblyLine NewAssemblyLine(bool underConstruction = true)
        {
            AssemblyLine a = new AssemblyLine();
            a.label = string.Format("Assembly Line {0}", MPmanager.manager.AssemblyLines.Count + 1);
            a.orderStack = new OrderStack(a);
            a.billOfMaterials = new BillOfMaterials(a);
            a.underConstruction = underConstruction;
            a.upgradeManager = new UpgradeManager(a);
            a.Speed = new AssemblyLineProperty(1f);
            a.Efficiency = new AssemblyLineProperty(1f);
            return a;
        }

        public AssemblyLineProperty GetProperty(UpgradeTarget target)
        {
            switch (target)
            {
                case UpgradeTarget.Efficiency:
                    return Efficiency;
                case UpgradeTarget.Speed:
                    return Speed;
                default:
                    {
                        Log.Error("Error getting property in assemblyline. Invalid target");
                        return null;
                    }
            }
        }

        public void AddUpgrade(AssemblyLineUpgrade upgrade)
        {
            AssemblyLineProperty property = GetProperty(upgrade.Def.PropertyToAffect);
            property -= upgrade.Def.PercentageDecrease;
        }

        public int PowerUsage
        {
            get
            {
                if (underConstruction && !constructionPaused)
                    return Settings.constructionPowerUsage;
                if (CurrentOrder != null && CurrentOrder.ShoppingList.HasAllMats)
                    return Settings.productionPowerUsage;
                return Settings.idlePowerUsage;
            }
        }

        public static int ConstructionTicksRequired
        {
            get
            {
                if (Game.godMode)
                {
                    if (AssemblyLine.Settings.instaBuild)
                        return 1;
                    else
                        return Settings.godModeBuildingLengthInt;
                }
                else
                    return Settings.buildingLengthInt;
            }
        }

        public virtual void Tick()
        {
            if (underConstruction)
            {
                if (!constructionPaused)
                {
                    if (!billOfMaterials.HasMats)
                    {
                        billOfMaterials.Tick();
                    }
                    else
                    {
                        if (DecreaseTime())
                        {
                            underConstruction = false;
                            Messages.Message(this.label + " has finished construction and is now ready for use!", MessageSound.Benefit);
                        }
                    }
                }
            }
            else
            {
                UpgradeManager.Tick();

                if (CurrentOrder != null)
                {
                    if (CurrentOrder.DesiresToWork)
                    {
                        CurrentOrder.Tick();
                    }
                    else
                    {
                        if (orderStack.Count > 1 && orderStack.Any((Order o) => o.DesiresToWork))
                            OrderStack.FinishOrderAndGetNext(CurrentOrder);
                    }
                }
            }
        }

        public void Delete()
        {
            if (underConstruction)
            {
                billOfMaterials.DropAcquiredMats();
            }
            if (CurrentOrder != null)
                CurrentOrder.Delete();
        }

        private bool DecreaseTime()
        {
            ConstructionTicks--;
            if (ConstructionTicks <= 0)
            {
                return true;
            }
            return false;
        }

        public UpgradeManager UpgradeManager
        {
            get
            {
                return this.upgradeManager;
            }
        }

        public int ConstructionTicks
        {
            get
            {
                return this.constructionTicks;
            }
            set
            {
                this.constructionTicks = value;
                if (constructionTicks < 0)
                    constructionTicks = 0;
            }
        }

        public void OnGUI(float curY, Vector2 lineEntrySize, Vector2 interactButtonSize, Layer page)
        {
            if (underConstruction)
            {
                //First we set the box that the current entry is in. We use the curY value to set its position in the list.
                Rect currentEntryRect = new Rect(0f, curY, lineEntrySize.x, lineEntrySize.y);

                //This widget draws the box for the entry.
                Widgets.DrawMenuSection(currentEntryRect);

                //We then get the innerRect of this entry box. This gives us a margin around the inside of the entry box, just to make it look nicer.
                Rect innerRect = currentEntryRect.ContractedBy(6f);

                //Start the group and display the information
                try
                {
                    GUI.BeginGroup(innerRect);

                    //This float is used for centering the buttons. The labels have the same height as the parent box, and the text inside them is aligned to the centre so this makes it easier for us to centre text.
                    float innerRectCentreY = (innerRect.yMax - innerRect.y) / 2;

                    //This sets up the rect for the line name label. It is indented 15f and is the same width and height as the parent box.
                    Rect lineLabelRect = new Rect(15f, 0f, innerRect.width, innerRect.height);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Text.Font = GameFont.Small;
                    Widgets.Label(lineLabelRect, this.label);

                    //We reset the color just to make sure it is still on white.
                    GUI.color = Color.white;

                    //This draws label which shows either under construction: time remaining, or manager required.
                    Rect lineStatusRect = new Rect(400f, 0f, innerRect.width, innerRect.height);
                    Text.Font = GameFont.Tiny;
                    //we change the color of the text here
                    GUI.color = new Color(1f, 1f, 1f, 0.5f);
                    string str;
                    if (billOfMaterials.HasMats)
                    {
                        str = string.Concat(new object[]{
                        "Under construction. Time remaining: ",
                        TicksToTime.GetTime((float)ConstructionTicks)
                    });
                    }
                    else
                    {
                        if (BillOfMaterials.MaterialsRequired.Count > 0)
                        {
                            str = "Requires materials:\n";
                            foreach (ListItem item in AssemblyLine.Settings.BuildingCost)
                            {
                                int num2 = billOfMaterials[item.thing];
                                str += num2 + "/" + item.amount + " " + item.thing.label + "\n";
                            }
                        }
                        else
                        {
                            str = "Nothing";
                        }
                    }
                    Widgets.Label(lineStatusRect, str);

                    //We change the color back to white
                    GUI.color = Color.white;
                    Text.Anchor = TextAnchor.UpperLeft;
                    Text.Font = GameFont.Small;

                    //This float is used to place the pause button. It takes the width of the parent box, takes away a margin of 12, then the width of the button and then the height. The height is used later for the delete button so this extra space is used.
                    float num = lineEntrySize.x - 12f - interactButtonSize.x - interactButtonSize.y;
                    //we use the centering float here to centre the buttons. Take away half the height of the button from the float to get the position where the button will be perfectly centered.
                    Rect butRect = new Rect(num, innerRectCentreY - (interactButtonSize.y / 2), interactButtonSize.x, interactButtonSize.y);
                    //Buttons return a bool if they are clicked, so placing them in an if statement will perform an action when they are clicked. (Remember that the FillWindow() function is called every frame, so this code is constantly being executed)
                    string butStr;
                    if (constructionPaused)
                        butStr = "Resume";
                    else
                        butStr = "Pause";
                    if (Widgets.TextButton(butRect, butStr))
                    {
                        if (constructionPaused)
                            constructionPaused = false;
                        else
                            constructionPaused = true;
                    }

                    Rect deleteButRect = new Rect(num + butRect.width + 5f, innerRectCentreY - (interactButtonSize.y / 2), interactButtonSize.y, interactButtonSize.y);
                    if (Widgets.ImageButton(deleteButRect, ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true)))
                    {
                        string s;
                        if (!underConstruction)
                        {
                            s = string.Concat(new object[]{
                            "WARNING\n\n",
                            "This will permanently delete this assembly line! Are you sure you wish to continue?"
                        });
                        }
                        else
                        {
                            s = string.Concat(new object[]{
                            "WARNING\n\n",
                            "This will permanently delete this assembly line! You will be refunded most of the materials it has acquired for construction.\n\n Are you sure you wish to continue?"
                            });
                        }
                        Find.LayerStack.Add(new Dialog_Confirm(s, delegate
                        {
                            MPmanager.manager.RemoveAssemblyLine(this);
                            ((Page_ManufacturingPlantMainUI)page).assemblyLines = MPmanager.manager.AssemblyLines;
                        }));
                    }
                    TooltipHandler.TipRegion(deleteButRect, "Delete");

                }
                finally
                {
                    GUI.EndGroup();
                }
            }
            else
            {
                //First we set the box that the current entry is in. We use the curY value to set its position in the list.
                Rect currentEntryRect = new Rect(0f, curY, lineEntrySize.x, lineEntrySize.y);

                //This widget draws the box for the entry.
                Widgets.DrawMenuSection(currentEntryRect);

                //We then get the innerRect of this entry box. This gives us a margin around the inside of the entry box, just to make it look nicer.
                Rect innerRect = currentEntryRect.ContractedBy(6f);

                //We then begin the group for the entry. This tells the system that all the following widgets are grouped together.
                GUI.BeginGroup(innerRect);

                //This float is used for centering the buttons. The labels have the same height as the parent box, and the text inside them is aligned to the centre so this makes it easier for us to centre text.
                float innerRectCentreY = (innerRect.yMax - innerRect.y) / 2;

                //This sets up the rect for the line name label. It is indented 15f and is the same width and height as the parent box.
                Rect lineLabelRect = new Rect(15f, 0f, innerRect.width, innerRect.height);
                Text.Anchor = TextAnchor.MiddleLeft;
                Text.Font = GameFont.Small;
                Widgets.Label(lineLabelRect, this.label);

                //We reset the color just to make sure it is still on white.
                GUI.color = Color.white;

                //This draws the manager info label. It is in the middle of the parent box.
                Rect lineInfoRect = new Rect(400f, 0f, innerRect.width, innerRect.height);
                Text.Font = GameFont.Tiny;
                //we change the color of the text here
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
                string statusString;
                if (this.CurrentOrder != null && CurrentOrder.DesiresToWork)
                {
                    if (CurrentOrder.ShoppingList.HasAllMats)
                        statusString = "Order in production";
                    else
                        statusString = "Order requires materials";
                }
                else
                {
                    statusString = "Idle";
                }
                Widgets.Label(lineInfoRect, statusString);

                //We change the color back to white
                GUI.color = Color.white;
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
                //This float is used to place the select button. It takes the width of the parent box, takes away a margin of 12, then the width of the button and then the height. The height is used later for the delete button so this extra space is used.
                float num = lineEntrySize.x - 12f - interactButtonSize.x - interactButtonSize.y;
                //we use the centering float here to centre the buttons. Take away half the height of the button from the float to get the position where the button will be perfectly centered.
                Rect butRect = new Rect(num, innerRectCentreY - (interactButtonSize.y / 2), interactButtonSize.x, interactButtonSize.y);
                //Buttons return a bool if they are clicked, so placing them in an if statement will perform an action when they are clicked. (Remember that the FillWindow() function is called every frame, so this code is constantly being executed)
                if (Widgets.TextButton(butRect, "Select"))
                {
                    Find.LayerStack.Add(new Page_LineManagementUI(this, page));
                }
                Rect deleteButRect = new Rect(num + butRect.width + 5f, innerRectCentreY - (interactButtonSize.y / 2), interactButtonSize.y, interactButtonSize.y);
                if (Widgets.ImageButton(deleteButRect, ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true)))
                {
                    string s = string.Concat(new object[]{
                            "WARNING\n\n",
                            "This will permanently delete this assembly line! Are you sure you wish to continue?"
                        });
                    Find.LayerStack.Add(new Dialog_Confirm(s, delegate
                    {
                        MPmanager.manager.RemoveAssemblyLine(this);
                        ((Page_ManufacturingPlantMainUI)page).assemblyLines = MPmanager.manager.AssemblyLines;
                    }));
                }
                TooltipHandler.TipRegion(deleteButRect, "Delete");
                GUI.EndGroup();
            }
        }

        public Order CurrentOrder
        {
            get
            {
                return this.orderStack.CurrentOrder;
            }
        }

        public OrderStack OrderStack
        {
            get
            {
                return this.orderStack;
            }
        }

        public virtual void Setup()
        {
            //TODO
            UpgradeManager.SpawnSetup();
        }

        public void ExposeData()
        {
            Scribe_Deep.LookDeep(ref this.Speed, "lineSpeed");
            Scribe_Deep.LookDeep(ref this.Efficiency, "lineEfficiency");
            Scribe_Deep.LookDeep<OrderStack>(ref this.orderStack, "orderStack", this);
            Scribe_Deep.LookDeep(ref this.upgradeManager, "upgradeManager", this);
            Scribe_Values.LookValue<string>(ref this.label, "lineLabel");
            Scribe_Values.LookValue(ref this.constructionTicks, "constructionTicks");
            Scribe_Values.LookValue(ref this.underConstruction, "underConstruction");
            Scribe_Values.LookValue(ref this.constructionPaused, "constructionPaused");
            Scribe_Deep.LookDeep(ref this.billOfMaterials, "billOfMaterials", this);
        }
    }
}
