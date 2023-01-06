mergeInto(LibraryManager.library, {

 syncSave: function() {
 FS.syncfs(false, function (err) {});
 },

 alert: function (message) {
    window.alert(Pointer_stringify(message));
  },

 });