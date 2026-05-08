mergeInto(LibraryManager.library, {
    LoadTOJamMetaData: function() {
        var retrieved = 'failed'

        // Adapted from example here: https://developer.mozilla.org/en-US/docs/Web/API/Web_Storage_API/Using_the_Web_Storage_API
        function isStorageAvailable() {
            var storage;
            try {
                storage = window.localStorage;
                const x = "__storage_test__";
                storage.setItem(x, x);
                storage.removeItem(x);
                return true;
            } catch (e) {
                return (
                    e instanceof DOMException &&
                    e.name === "QuotaExceededError" &&
                    // acknowledge QuotaExceededError only if there's something already stored
                    storage &&
                    storage.length !== 0
                );
            }
        }        

        if (isStorageAvailable()) {
            retrieved = window.localStorage.getItem('tojam2025-metagamedata');
            //console.log('METAGAME - Successfully read data:', retrieved);
        } else {
            console.log('METAGAME - Could not use localStorage for metagame content');
        }

        if (!retrieved) {
            console.log('METAGAME - Loaded data was blank:', retrieved);
            retrieved = '';            
        }

        const bufferSize = lengthBytesUTF8(retrieved) + 1;
        const buffer = _malloc(bufferSize);
        stringToUTF8(retrieved, buffer, bufferSize);
        return buffer;
    },

    SaveTOJamMetaData: function(jsonText) {
        try {
            window.localStorage.setItem('tojam2025-metagamedata', UTF8ToString(jsonText));
        } catch (e) {
            console.log(`METAGAME - Failed to save updated metagame info.`, e)
        }
    }
});