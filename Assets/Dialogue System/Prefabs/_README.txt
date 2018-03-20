/*
This folder contains prefabs that you can use in your projects. Prefabs inside subfolders named
"Resources" may be automatically loaded by the Dialogue System as default values if your project 
hasn't set the dialogue UI or camera angles prefab.

Dialogue Manager: This is a generic Dialogue Manager prefab that uses Unity UI. If you add it to
your scene, you must assign a dialogue database to its Initial Database field. You may also want
to add an EventSystem to your scene. 

   This prefab does *not* include a Selector UI, since not all games will use it. To add a 
   Selector UI, add Unity UI Prefabs/Generic/Generic Unity UI Selector Panel to the Canvas, 
   or replace the entire Canvas with Generic Bundled UI. Your player should have a Selector
   or Proximity Selector component and a Unity UI Selector Display component.

Articy Template/articy_template.zip: A template articy:draft 2.4 project designed for the
Dialogue System.

Chat Mapper Template/Template.cmp: A template Chat Mapper project designed for the Dialogue System.

Camera Angles: Contains a prefab with a selection of camera angles that you can reference in
your cutscene sequences.

Unity UI Prefabs and Legacy Unity GUI Prefabs: Contain a large selection of Unity UI-based and 
legacy Unity GUI-based dialogue UIs that you can use to set the Dialogue System's appearance in 
your project. You can delete any you don't need, but don't delete the Default folder in 
Legacy Unity GUI Prefabs; this contains the default fallback UI that the Dialogue System uses if 
it can't access your regular UI.

iOS: Contains platform-specific utility files for iOS projects.
*/