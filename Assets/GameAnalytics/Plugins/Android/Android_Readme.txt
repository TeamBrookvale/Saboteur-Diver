Google has introduced the Advertising ID, which requires Google Play services (https://developer.android.com/google/play-services/id.html). This means that things have changed a little when it comes to using GameAnalytics for Unity Android. Fortunately, by default you do not have to do anything, but if you already have an AndroidManifest.xml file in your Plugins/Android folder, then you will have to add the following line to that manifest (this line must be added inside the application tag):

<meta-data android:name="com.google.android.gms.version" android:value="4030500" />

Note: if you already have your own AndroidManifest.xml, then you should simply remove the default manifest that comes with the GameAnalytics package, after you have copied the line above into your manifest file.