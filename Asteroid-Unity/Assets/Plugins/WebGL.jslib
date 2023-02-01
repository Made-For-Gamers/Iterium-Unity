mergeInto(LibraryManager.library, 
{
    //Force update of IndexedDB immediately when actioning a save game
    syncSave: function ()
    {
        FS.syncfs(false, function (err) {});
    },

    //Alert popup to display a message to the user or debugging
    alert: function (message)
    {
        window.alert(Pointer_stringify(message));
    },
});