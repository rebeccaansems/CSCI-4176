using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// NPC setup wizard.
    /// </summary>
    public class NPCSetupWizard : EditorWindow
    {

        [MenuItem("Window/Dialogue System/Wizards/NPC Setup", false, 1)]
        public static void Init()
        {
            (EditorWindow.GetWindow(typeof(NPCSetupWizard), false, "NPC Setup") as NPCSetupWizard).minSize = new Vector2(700, 500);
        }

        // Private fields for the window:

        private enum Stage
        {
            SelectNPC,
            SelectDB,
            Conversation,
            Bark,
            Targeting,
            Persistence,
            Review
        };

        private Stage stage = Stage.SelectNPC;

        private string[] stageLabels = new string[] { "NPC", "Database", "Conversation", "Bark", "Targeting", "Persistence", "Review" };

        private const float ToggleWidth = 16;

        private GameObject npcObject = null;

        private DialogueDatabase database = null;
        private string[] conversationList = null;
        private DialogueSystemController dialogueManager = null;

        /// <summary>
        /// Draws the window.
        /// </summary>
        void OnGUI()
        {
            DrawProgressIndicator();
            DrawCurrentStage();
        }

        private void DrawProgressIndicator()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Toolbar((int)stage, stageLabels, GUILayout.Width(700));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            EditorWindowTools.DrawHorizontalLine();
        }

        private void DrawNavigationButtons(bool backEnabled, bool nextEnabled, bool nextCloses)
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", GUILayout.Width(100)))
            {
                this.Close();
            }
            else if (backEnabled && GUILayout.Button("Back", GUILayout.Width(100)))
            {
                stage--;
            }
            else
            {
                EditorGUI.BeginDisabledGroup(!nextEnabled);
                if (GUILayout.Button(nextCloses ? "Finish" : "Next", GUILayout.Width(100)))
                {
                    if (nextCloses)
                    {
                        Close();
                    }
                    else
                    {
                        stage++;
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField(string.Empty, GUILayout.Height(2));
        }

        private void DrawCurrentStage()
        {
            if (npcObject == null) stage = Stage.SelectNPC;
            switch (stage)
            {
                case Stage.SelectNPC: DrawSelectNPCStage(); break;
                case Stage.SelectDB: DrawSelectDBStage(); break;
                case Stage.Conversation: DrawConversationStage(); break;
                case Stage.Bark: DrawBarkStage(); break;
                case Stage.Targeting: DrawTargetingStage(); break;
                case Stage.Persistence: DrawPersistenceStage(); break;
                case Stage.Review: DrawReviewStage(); break;
            }
            if ((npcObject != null) && GUI.changed) EditorUtility.SetDirty(npcObject);
        }

        private void DrawSelectNPCStage()
        {
            EditorGUILayout.LabelField("Select NPC Object", EditorStyles.boldLabel);
            EditorWindowTools.StartIndentedSection();
            EditorGUILayout.HelpBox("This wizard will help you configure a Non Player Character (NPC) object to work with the Dialogue System. First, assign the NPC's GameObject below.", MessageType.Info);
            npcObject = EditorGUILayout.ObjectField("NPC Object", npcObject, typeof(GameObject), true) as GameObject;
            EditorWindowTools.EndIndentedSection();
            DrawNavigationButtons(false, (npcObject != null), false);
        }

        private void DrawSelectDBStage()
        {
            EditorGUILayout.LabelField("Select Dialogue Database", EditorStyles.boldLabel);
            EditorWindowTools.StartIndentedSection();
            EditorGUILayout.HelpBox("Assign the dialogue database that the NPC will use for conversations and barks.", MessageType.Info);
            if (dialogueManager == null)
            {
                dialogueManager = FindObjectOfType<DialogueSystemController>();
            }
            if (database == null && dialogueManager != null)
            {
                database = dialogueManager.initialDatabase;
            }
            DialogueDatabase newDatabase = EditorGUILayout.ObjectField("Dialogue Database", database, typeof(DialogueDatabase), true) as DialogueDatabase;
            if (newDatabase != database)
            {
                database = newDatabase;
                CreateConversationList(database);
            }
            if (dialogueManager != null && database != null && dialogueManager.initialDatabase != database)
            {
                EditorGUILayout.HelpBox("This is not the initial database assigned to the Dialogue Manager. Remember to load this database at runtime before using the NPC. You can use the Extra Databases component or DialogueManager.AddDatabase() in script. Also make sure the internal IDs are unique across databases. You can use the Unique ID Tool to do this.", MessageType.Warning);
            }
            EditorWindowTools.EndIndentedSection();
            DrawNavigationButtons(true, (database != null), false);
        }

        private void CreateConversationList(DialogueDatabase database)
        {
            List<string> list = new List<string>();
            if (database != null)
            {
                list.Add(string.Empty);
                foreach (var conversation in database.conversations)
                {
                    list.Add(conversation.Title);
                }
            }
            conversationList = list.ToArray();
        }

        private string DrawConversationPopup(string title)
        {
            if (conversationList == null) CreateConversationList(database);
            if (conversationList == null) return string.Empty;
            int index = GetConversationIndex(title);
            index = EditorGUILayout.Popup("Conversation", index, conversationList);
            return conversationList[index];
        }

        private int GetConversationIndex(string title)
        {
            for (int i = 0; i < conversationList.Length; i++)
            {
                if (string.Equals(title, conversationList[i])) return i;
            }
            return 0;
        }

        private void DrawConversationStage()
        {
            EditorGUILayout.LabelField("Conversation", EditorStyles.boldLabel);
            ConversationTrigger conversationTrigger = npcObject.GetComponentInChildren<ConversationTrigger>();
            EditorWindowTools.StartIndentedSection();
            EditorGUILayout.HelpBox("If the NPC has an interactive conversation (for example, with the player), tick the checkbox below.", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            bool hasConversation = EditorGUILayout.Toggle((conversationTrigger != null), GUILayout.Width(ToggleWidth));
            EditorGUILayout.LabelField("NPC has an interactive conversation", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (hasConversation)
            {
                EditorWindowTools.StartIndentedSection();
                if (conversationTrigger == null) conversationTrigger = npcObject.AddComponent<ConversationTrigger>();
                EditorGUILayout.HelpBox("Select the NPC's conversation and what event will trigger it. If the NPC will only engage in this conversation once, tick Once Only. Click Select NPC to further customize the Conversation Trigger in the inspector, such as setting conditions on when the conversation can occur.",
                    string.IsNullOrEmpty(conversationTrigger.conversation) ? MessageType.Info : MessageType.None);
                conversationTrigger.conversation = DrawConversationPopup(conversationTrigger.conversation);
                conversationTrigger.once = EditorGUILayout.Toggle("Once Only", conversationTrigger.once);
                conversationTrigger.trigger = DrawTriggerPopup(conversationTrigger.trigger);
                if (GUILayout.Button("Select NPC", GUILayout.Width(100))) Selection.activeGameObject = npcObject;
                EditorWindowTools.EndIndentedSection();
            }
            else
            {
                DestroyImmediate(conversationTrigger);
            }
            EditorWindowTools.EndIndentedSection();
            DrawNavigationButtons(true, true, false);
        }

        private DialogueTriggerEvent DrawTriggerPopup(DialogueTriggerEvent trigger)
        {
            //---Was: DialogueTriggerEvent result = (DialogueTriggerEvent) EditorGUILayout.EnumPopup("Trigger", trigger);
            var result = DialogueTriggerEventDrawer.LayoutPopup("Trigger", trigger);
            EditorGUILayout.HelpBox(GetDialogueTriggerDescription(trigger), MessageType.None);
            return result;
        }

        private string GetDialogueTriggerDescription(DialogueTriggerEvent trigger)
        {
            switch (trigger)
            {
                case DialogueTriggerEvent.OnBarkEnd: return "Will occur when the NPC receives an 'OnBarkEnd' message.";
                case DialogueTriggerEvent.OnConversationEnd: return "Will occur when the NPC receives an 'OnConversationEnd' message.";
                case DialogueTriggerEvent.OnEnable: return "Will occur when the NPC's components are enabled (or re-enabled).";
                case DialogueTriggerEvent.OnSequenceEnd: return "Will occur when the NPC receives an 'OnSequenceEnd' message.";
                case DialogueTriggerEvent.OnStart: return "Will occur when the NPC object is started.";
                case DialogueTriggerEvent.OnTriggerEnter: return "Will occur when a valid actor (usually the player) enters the NPC's trigger collider.";
                case DialogueTriggerEvent.OnUse: return "Will occur when the NPC receives an 'OnUse' event. If the player has a Selector component, it can send OnUse. You can also send OnUse from scripts, cutscene sequences, etc.";
                case DialogueTriggerEvent.OnDestroy: return "Will occur when the NPC object is destroyed.";
                case DialogueTriggerEvent.OnDisable: return "Will occur when the corresponding component on the NPC is disabled.";
                case DialogueTriggerEvent.OnTriggerExit: return "Will occur when a valid actor (usually the player) exits the NPC's trigger collider.";
                case DialogueTriggerEvent.None: return "Will not be triggered automatically.";
                default: return string.Empty;
            }
        }

        private void DrawBarkStage()
        {
            EditorGUILayout.LabelField("Bark", EditorStyles.boldLabel);
            EditorWindowTools.StartIndentedSection();
            EditorGUILayout.HelpBox("If the NPC barks (says one-off lines during gameplay), tick either or both of the checkboxs below.", MessageType.Info);
            bool hasBarkTrigger = DrawBarkTriggerSection();
            EditorWindowTools.DrawHorizontalLine();
            bool hasBarkOnIdle = DrawBarkOnIdleSection();
            bool hasBarkComponent = hasBarkTrigger || hasBarkOnIdle;
            bool hasBarkUI = false;
            if (hasBarkComponent) hasBarkUI = DrawBarkUISection();
            if (hasBarkComponent && GUILayout.Button("Select NPC", GUILayout.Width(100))) Selection.activeGameObject = npcObject;
            EditorWindowTools.EndIndentedSection();
            DrawNavigationButtons(true, (hasBarkUI || !hasBarkComponent), false);
        }

        private bool DrawBarkTriggerSection()
        {
            EditorGUILayout.BeginHorizontal();
            BarkTrigger barkTrigger = npcObject.GetComponentInChildren<BarkTrigger>();
            bool hasBarkTrigger = EditorGUILayout.Toggle((barkTrigger != null), GUILayout.Width(ToggleWidth));
            EditorGUILayout.LabelField("NPC barks when triggered", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (hasBarkTrigger)
            {
                EditorWindowTools.StartIndentedSection();
                if (barkTrigger == null) barkTrigger = npcObject.AddComponent<BarkTrigger>();
                EditorGUILayout.HelpBox("Select the conversation containing the NPC's bark lines, the order in which to display them, and when barks should be triggered.", string.IsNullOrEmpty(barkTrigger.conversation) ? MessageType.Info : MessageType.None);
                barkTrigger.conversation = DrawConversationPopup(barkTrigger.conversation);
                barkTrigger.barkOrder = (BarkOrder)EditorGUILayout.EnumPopup("Order of Lines", barkTrigger.barkOrder);
                barkTrigger.trigger = DrawTriggerPopup(barkTrigger.trigger);
                EditorWindowTools.EndIndentedSection();
            }
            else
            {
                DestroyImmediate(barkTrigger);
            }
            return hasBarkTrigger;
        }

        private bool DrawBarkOnIdleSection()
        {
            EditorGUILayout.BeginHorizontal();
            BarkOnIdle barkOnIdle = npcObject.GetComponentInChildren<BarkOnIdle>();
            bool hasBarkOnIdle = EditorGUILayout.Toggle((barkOnIdle != null), GUILayout.Width(ToggleWidth));
            EditorGUILayout.LabelField("NPC barks on a timed basis", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (hasBarkOnIdle)
            {
                EditorWindowTools.StartIndentedSection();
                if (barkOnIdle == null) barkOnIdle = npcObject.AddComponent<BarkOnIdle>();
                EditorGUILayout.HelpBox("Select the conversation containing the NPC's bark lines, the order in which to display them, and the time that should pass between barks.",
                    string.IsNullOrEmpty(barkOnIdle.conversation) ? MessageType.Info : MessageType.None);
                barkOnIdle.conversation = DrawConversationPopup(barkOnIdle.conversation);
                barkOnIdle.barkOrder = (BarkOrder)EditorGUILayout.EnumPopup("Order of Lines", barkOnIdle.barkOrder);
                barkOnIdle.minSeconds = EditorGUILayout.FloatField("Min Seconds", barkOnIdle.minSeconds);
                barkOnIdle.maxSeconds = EditorGUILayout.FloatField("Max Seconds", barkOnIdle.maxSeconds);
                EditorWindowTools.EndIndentedSection();
            }
            else
            {
                DestroyImmediate(barkOnIdle);
            }
            return hasBarkOnIdle;
        }

        private bool DrawBarkUISection()
        {
            EditorWindowTools.DrawHorizontalLine();
            EditorGUILayout.BeginHorizontal();
            IBarkUI barkUI = npcObject.GetComponentInChildren(typeof(IBarkUI)) as IBarkUI;
            bool hasBarkUI = (barkUI != null);
            EditorGUILayout.LabelField("Bark UI", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            EditorWindowTools.StartIndentedSection();
            if (hasBarkUI)
            {
                EditorGUILayout.HelpBox("The NPC has a bark UI, so it will be able to display barks.", MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox("The NPC needs a bark UI to be able to display barks. Click Default Bark UI to add a default Unity GUI bark UI, or assign one manually from Window > Dialogue System > Component > UI.", MessageType.Info);
                if (GUILayout.Button("Default Bark UI", GUILayout.Width(160)))
                {
                    npcObject.AddComponent<PixelCrushers.DialogueSystem.UnityGUI.UnityBarkUI>();
                    hasBarkUI = true;
                }
            }
            EditorWindowTools.EndIndentedSection();
            return hasBarkUI;
        }

        private bool use2D = false;

        private void DrawTargetingStage()
        {
            DrawOverrideNameSubsection(npcObject);
            EditorGUILayout.LabelField("Targeting", EditorStyles.boldLabel);
            EditorWindowTools.StartIndentedSection();
            use2D = EditorGUILayout.ToggleLeft(new GUIContent("2D", "Tick for 2D scenes, untick for 3D scenes."), use2D);
            ConversationTrigger conversationTrigger = npcObject.GetComponentInChildren<ConversationTrigger>();
            BarkTrigger barkTrigger = npcObject.GetComponentInChildren<BarkTrigger>();
            bool hasOnTriggerEnter = ((conversationTrigger != null) && (conversationTrigger.trigger == DialogueTriggerEvent.OnTriggerEnter)) ||
                ((barkTrigger != null) && (barkTrigger.trigger == DialogueTriggerEvent.OnTriggerEnter));
            bool hasOnUse = ((conversationTrigger != null) && (conversationTrigger.trigger == DialogueTriggerEvent.OnUse)) ||
                ((barkTrigger != null) && (barkTrigger.trigger == DialogueTriggerEvent.OnUse));
            bool needsColliders = hasOnTriggerEnter || hasOnUse;
            bool hasAppropriateColliders = false;
            if (hasOnTriggerEnter) hasAppropriateColliders = DrawTargetingOnTriggerEnter();
            if (hasOnUse) hasAppropriateColliders = DrawTargetingOnUse() || hasAppropriateColliders;
            if (!needsColliders) EditorGUILayout.HelpBox("The NPC doesn't need any targeting components. Click Next to proceed.", MessageType.Info);
            if (GUILayout.Button("Select NPC", GUILayout.Width(100))) Selection.activeGameObject = npcObject;
            EditorWindowTools.EndIndentedSection();
            DrawNavigationButtons(true, (hasAppropriateColliders || !needsColliders), false);
        }

        private bool DrawTargetingOnTriggerEnter()
        {
            EditorWindowTools.DrawHorizontalLine();
            return use2D ? Draw2DTargetingOnTriggerEnter() : Draw3DTargetingOnTriggerEnter();
        }

        private bool Draw2DTargetingOnTriggerEnter()
        {
            EditorGUILayout.LabelField("Trigger Collider", EditorStyles.boldLabel);
            Collider2D triggerCollider = null;
            foreach (var collider in npcObject.GetComponentsInChildren<Collider2D>())
            {
                if (collider.isTrigger) triggerCollider = collider;
            }
            if (triggerCollider != null)
            {
                if (triggerCollider is CircleCollider2D)
                {
                    EditorGUILayout.HelpBox("The NPC has a trigger collider, so it's ready for OnTriggerEnter events. You can adjust its radius below. Make sure its layer collision properties are configured to detect when the intended colliders enter its area.", MessageType.None);
                    var circleCollider = triggerCollider as CircleCollider2D;
                    circleCollider.radius = EditorGUILayout.FloatField("Radius", circleCollider.radius);
                    return true;
                }
                else
                {
                    EditorGUILayout.HelpBox("The NPC has a trigger collider, so it's ready for OnTriggerEnter events.", MessageType.None);
                }
                return true;
            }
            else
            {
                EditorGUILayout.HelpBox("The NPC needs a trigger collider. Add Trigger to add a circle collider trigger, or add one manually.", MessageType.Info);
                if (GUILayout.Button("Add Trigger", GUILayout.Width(160)))
                {
                    var circleCollider = npcObject.AddComponent<CircleCollider2D>();
                    circleCollider.isTrigger = true;
                    circleCollider.radius = 1.5f;
                    var rb = npcObject.GetComponent<Rigidbody2D>();
                    if (rb == null)
                    {
                        rb = npcObject.AddComponent<Rigidbody2D>();
                        rb.isKinematic = true;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool Draw3DTargetingOnTriggerEnter()
        {
            EditorGUILayout.LabelField("Trigger Collider", EditorStyles.boldLabel);
            Collider triggerCollider = null;
            foreach (var collider in npcObject.GetComponentsInChildren<Collider>())
            {
                if (collider.isTrigger) triggerCollider = collider;
            }
            if (triggerCollider != null)
            {
                if (triggerCollider is SphereCollider)
                {
                    EditorGUILayout.HelpBox("The NPC has a trigger collider, so it's ready for OnTriggerEnter events. You can adjust its radius below. Make sure its layer collision properties are configured to detect when the intended colliders enter its area.", MessageType.None);
                    SphereCollider sphereCollider = triggerCollider as SphereCollider;
                    sphereCollider.radius = EditorGUILayout.FloatField("Radius", sphereCollider.radius);
                    return true;
                }
                else
                {
                    EditorGUILayout.HelpBox("The NPC has a trigger collider, so it's ready for OnTriggerEnter events.", MessageType.None);
                }
                return true;
            }
            else
            {
                EditorGUILayout.HelpBox("The NPC needs a trigger collider. Add Trigger to add a sphere trigger, or add one manually.", MessageType.Info);
                if (GUILayout.Button("Add Trigger", GUILayout.Width(160)))
                {
                    SphereCollider sphereCollider = npcObject.AddComponent<SphereCollider>();
                    sphereCollider.isTrigger = true;
                    sphereCollider.radius = 1.5f;
                    var rb = npcObject.GetComponent<Rigidbody>();
                    if (rb == null)
                    {
                        rb = npcObject.AddComponent<Rigidbody>();
                        rb.isKinematic = true;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool DrawTargetingOnUse()
        {
            EditorWindowTools.DrawHorizontalLine();
            return use2D ? Draw2DTargetingOnUse() : Draw3DTargetingOnUse();
        }

        private bool Draw2DTargetingOnUse()
        {
            EditorGUILayout.LabelField("Collider", EditorStyles.boldLabel);
            Collider2D nontriggerCollider = null;
            Collider2D triggerCollider = null;
            foreach (var collider in npcObject.GetComponentsInChildren<Collider2D>())
            {
                if (collider.isTrigger)
                {
                    triggerCollider = collider;
                }
                else
                {
                    nontriggerCollider = collider;
                }
            }
            Usable usable = npcObject.GetComponent<Usable>();
            if ((nontriggerCollider != null) && (usable != null))
            {
                EditorGUILayout.HelpBox("The NPC has a collider and a Usable component. The player's Selector component will be able to target it to send OnUse messages.", MessageType.None);
                EditorGUILayout.LabelField("'Usable' Customization", EditorStyles.boldLabel);
                EditorWindowTools.StartIndentedSection();
                usable.maxUseDistance = EditorGUILayout.FloatField("Max Usable Distance", usable.maxUseDistance);
                usable.overrideName = EditorGUILayout.TextField("Override Actor Name (leave blank to use main override)", usable.overrideName);
                usable.overrideUseMessage = EditorGUILayout.TextField("Override Use Message", usable.overrideUseMessage);
                EditorWindowTools.EndIndentedSection();
            }
            else
            {
                if (nontriggerCollider == null)
                {
                    EditorGUILayout.HelpBox("The NPC is configured to listen for OnUse messages. If these messages will be coming from the player's Selector component, the NPC needs a collider that the Selector can target. Click Add Collider to add a CharacterController. If OnUse will come from another source, you don't necessarily need a collider.", MessageType.Info);
                    if (GUILayout.Button("Add Collider", GUILayout.Width(160)))
                    {
                        npcObject.AddComponent<BoxCollider2D>();
                    }
                }
                if (triggerCollider is CircleCollider2D)
                {
                    EditorGUILayout.HelpBox("The NPC has a trigger collider, so it's ready for OnUse events from the player's ProximitySelector if it has one. You can adjust its radius below. Make sure its layer collision properties are configured to detect when the intended colliders enter its area.", MessageType.None);
                    var circleCollider = triggerCollider as CircleCollider2D;
                    circleCollider.radius = EditorGUILayout.FloatField("Radius", circleCollider.radius);
                    return true;
                }
                if (triggerCollider == null)
                {
                    EditorGUILayout.HelpBox("The NPC is configured to listen for OnUse messages. If these messages will be coming from the player's ProximitySelector component, the NPC needs a trigger collider that the ProximitySelector can detect. Click Add Trigger Collider to add a Circle Collider. If OnUse will come from another source, you don't necessarily need a trigger collider.", MessageType.Info);
                    if (GUILayout.Button("Add Trigger Collider", GUILayout.Width(160)))
                    {
                        triggerCollider = npcObject.AddComponent<CircleCollider2D>();
                        triggerCollider.isTrigger = true;
                        var rb = npcObject.GetComponent<Rigidbody2D>();
                        if (rb == null)
                        {
                            rb = npcObject.AddComponent<Rigidbody2D>();
                            rb.isKinematic = true;
                        }
                    }

                }
                if (usable == null)
                {
                    EditorGUILayout.HelpBox("The NPC is configured to listen for OnUse messages. If these messages will be coming from the player's Selector component, the NPC needs a Usable component to tell the Selector that it's usable. Click Add Usable to add one.", MessageType.Info);
                    if (GUILayout.Button("Add Usable", GUILayout.Width(160)))
                    {
                        npcObject.AddComponent<Usable>();
                    }
                }
            }
            return true;
        }

        private bool Draw3DTargetingOnUse()
        {
            EditorGUILayout.LabelField("Collider", EditorStyles.boldLabel);
            Collider nontriggerCollider = null;
            Collider triggerCollider = null;
            foreach (var collider in npcObject.GetComponentsInChildren<Collider>())
            {
                if (collider.isTrigger)
                {
                    triggerCollider = collider;
                }
                else
                {
                    nontriggerCollider = collider;
                }
            }
            Usable usable = npcObject.GetComponent<Usable>();
            if ((nontriggerCollider != null) && (usable != null))
            {
                EditorGUILayout.HelpBox("The NPC has a collider and a Usable component. The player's Selector component will be able to target it to send OnUse messages.", MessageType.None);
                EditorGUILayout.LabelField("'Usable' Customization", EditorStyles.boldLabel);
                EditorWindowTools.StartIndentedSection();
                usable.maxUseDistance = EditorGUILayout.FloatField("Max Usable Distance", usable.maxUseDistance);
                usable.overrideName = EditorGUILayout.TextField("Override Actor Name (leave blank to use main override)", usable.overrideName);
                usable.overrideUseMessage = EditorGUILayout.TextField("Override Use Message", usable.overrideUseMessage);
                EditorWindowTools.EndIndentedSection();
            }
            else
            {
                if (nontriggerCollider == null)
                {
                    EditorGUILayout.HelpBox("The NPC is configured to listen for OnUse messages. If these messages will be coming from the player's Selector component, the NPC needs a collider that the Selector can target. Click Add Collider to add a CharacterController. If OnUse will come from another source, you don't necessarily need a collider.", MessageType.Info);
                    if (GUILayout.Button("Add Collider", GUILayout.Width(160)))
                    {
                        npcObject.AddComponent<CharacterController>();
                    }
                }
                if (triggerCollider == null)
                {
                    EditorGUILayout.HelpBox("The NPC is configured to listen for OnUse messages. If these messages will be coming from the player's ProximitySelector component, the NPC needs a trigger collider that the ProximitySelector can detect. Click Add Trigger Collider to add a Sphere Collider. If OnUse will come from another source, you don't necessarily need a trigger collider.", MessageType.Info);
                    if (GUILayout.Button("Add Trigger Collider", GUILayout.Width(160)))
                    {
                        triggerCollider = npcObject.AddComponent<SphereCollider>();
                        triggerCollider.isTrigger = true;
                        var rb = npcObject.GetComponent<Rigidbody>();
                        if (rb == null)
                        {
                            rb = npcObject.AddComponent<Rigidbody>();
                            rb.isKinematic = true;
                        }
                    }
                }
                else if (triggerCollider is SphereCollider)
                {
                    EditorGUILayout.HelpBox("The NPC has a trigger collider, so it's ready for OnUse events from the player's ProximitySelector if it has one. You can adjust its radius below. Make sure its layer collision properties are configured to detect when the intended colliders enter its area.", MessageType.None);
                    SphereCollider sphereCollider = triggerCollider as SphereCollider;
                    sphereCollider.radius = EditorGUILayout.FloatField("Radius", sphereCollider.radius);
                }
                if (usable == null)
                {
                    EditorGUILayout.HelpBox("The NPC is configured to listen for OnUse messages. If these messages will be coming from the player's Selector component, the NPC needs a Usable component to tell the Selector that it's usable. Click Add Usable to add one.", MessageType.Info);
                    if (GUILayout.Button("Add Usable", GUILayout.Width(160)))
                    {
                        npcObject.AddComponent<Usable>();
                    }
                }
            }
            return true;
        }

        public static void DrawOverrideNameSubsection(GameObject character)
        {
            EditorGUILayout.LabelField("Override Actor Name", EditorStyles.boldLabel);
            OverrideActorName overrideActorName = character.GetComponent<OverrideActorName>();
            EditorWindowTools.StartIndentedSection();
            EditorGUILayout.HelpBox(string.Format("By default, the dialogue UI will use the name of the GameObject ({0}). You can override it below.", character.name), MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            bool hasOverrideActorName = EditorGUILayout.Toggle((overrideActorName != null), GUILayout.Width(ToggleWidth));
            EditorGUILayout.LabelField("Override actor name", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (hasOverrideActorName)
            {
                if (overrideActorName == null) overrideActorName = character.AddComponent<OverrideActorName>();
                overrideActorName.overrideName = EditorGUILayout.TextField("Actor Name", overrideActorName.overrideName);
            }
            else
            {
                DestroyImmediate(overrideActorName);
            }
            EditorWindowTools.EndIndentedSection();
            EditorWindowTools.DrawHorizontalLine();
        }

        private void DrawPersistenceStage()
        {
            EditorGUILayout.LabelField("Persistence", EditorStyles.boldLabel);
            PersistentPositionData persistentPositionData = npcObject.GetComponent<PersistentPositionData>();
            EditorWindowTools.StartIndentedSection();
            EditorGUILayout.HelpBox("The NPC can be configured to record its position in the Dialogue System's Lua environment so it will be preserved when saving and loading games.", MessageType.Info);
            EditorGUILayout.BeginHorizontal();
            bool hasPersistentPosition = EditorGUILayout.Toggle((persistentPositionData != null), GUILayout.Width(ToggleWidth));
            EditorGUILayout.LabelField("NPC records position for saved games", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (hasPersistentPosition)
            {
                if (persistentPositionData == null) persistentPositionData = npcObject.AddComponent<PersistentPositionData>();
                if (string.IsNullOrEmpty(persistentPositionData.overrideActorName))
                {
                    EditorGUILayout.HelpBox(string.Format("Position data will be saved to the Actor['{0}'] (the name of the NPC GameObject) or the Override Actor Name if defined. You can override the name below.", npcObject.name), MessageType.None);
                }
                else
                {
                    EditorGUILayout.HelpBox(string.Format("Position data will be saved to the Actor['{0}']. To use the name of the NPC GameObject instead, clear the field below.", persistentPositionData.overrideActorName), MessageType.None);
                }
                persistentPositionData.overrideActorName = EditorGUILayout.TextField("Actor Name", persistentPositionData.overrideActorName);
            }
            else
            {
                DestroyImmediate(persistentPositionData);
            }
            if (GUILayout.Button("Select NPC", GUILayout.Width(100))) Selection.activeGameObject = npcObject;
            EditorWindowTools.EndIndentedSection();
            DrawNavigationButtons(true, true, false);
        }

        private void DrawReviewStage()
        {
            EditorGUILayout.LabelField("Review", EditorStyles.boldLabel);
            EditorWindowTools.StartIndentedSection();
            EditorGUILayout.HelpBox("Your NPC is ready! Below is a summary of your NPC's configuration.\n\nNPC configuration can sometimes be complex. This wizard can't anticipate every possible character design. If your NPC doesn't behave the way you expect, please examine the components on the NPC and make adjustments as necessary.", MessageType.Info);
            ConversationTrigger conversationTrigger = npcObject.GetComponentInChildren<ConversationTrigger>();
            if (conversationTrigger != null)
            {
                EditorGUILayout.LabelField(string.Format("Conversation: '{0}'{1} {2}", conversationTrigger.conversation, conversationTrigger.once ? " (once)" : string.Empty, conversationTrigger.trigger));
            }
            else
            {
                EditorGUILayout.LabelField("Conversation: None");
            }
            BarkTrigger barkTrigger = npcObject.GetComponentInChildren<BarkTrigger>();
            if (barkTrigger != null)
            {
                EditorGUILayout.LabelField(string.Format("Triggered Bark: '{0}' ({1}) {2}", barkTrigger.conversation, barkTrigger.barkOrder, barkTrigger.trigger));
            }
            else
            {
                EditorGUILayout.LabelField("Triggered Bark: None");
            }
            BarkOnIdle barkOnIdle = npcObject.GetComponentInChildren<BarkOnIdle>();
            if (barkOnIdle != null)
            {
                EditorGUILayout.LabelField(string.Format("Timed Bark: '{0}' ({1}) every {2}-{3} seconds", barkOnIdle.conversation, barkOnIdle.barkOrder, barkOnIdle.minSeconds, barkOnIdle.maxSeconds));
            }
            else
            {
                EditorGUILayout.LabelField("Timed Bark: No");
            }
            PersistentPositionData persistentPositionData = npcObject.GetComponentInChildren<PersistentPositionData>();
            EditorGUILayout.LabelField(string.Format("Save Position: {0}", (persistentPositionData != null) ? "Yes" : "No"));
            EditorWindowTools.EndIndentedSection();
            DrawNavigationButtons(true, true, true);
        }

    }

}
