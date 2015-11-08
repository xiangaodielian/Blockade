
var Module;
if (typeof Module === 'undefined') Module = eval('(function() { try { return Module || {} } catch(e) { return {} } })()');
if (!Module.expectedDataFileDownloads) {
  Module.expectedDataFileDownloads = 0;
  Module.finishedDataFileDownloads = 0;
}
Module.expectedDataFileDownloads++;
(function() {

    var PACKAGE_PATH;
    if (typeof window === 'object') {
      PACKAGE_PATH = window['encodeURIComponent'](window.location.pathname.toString().substring(0, window.location.pathname.toString().lastIndexOf('/')) + '/');
    } else if (typeof location !== 'undefined') {
      // worker
      PACKAGE_PATH = encodeURIComponent(location.pathname.toString().substring(0, location.pathname.toString().lastIndexOf('/')) + '/');
    } else {
      throw 'using preloaded data can only be done on a web page or in a web worker';
    }
    var PACKAGE_NAME = 'WebGL.data';
    var REMOTE_PACKAGE_BASE = 'WebGL.data';
    if (typeof Module['locateFilePackage'] === 'function' && !Module['locateFile']) {
      Module['locateFile'] = Module['locateFilePackage'];
      Module.printErr('warning: you defined Module.locateFilePackage, that has been renamed to Module.locateFile (using your locateFilePackage for now)');
    }
    var REMOTE_PACKAGE_NAME = typeof Module['locateFile'] === 'function' ?
                              Module['locateFile'](REMOTE_PACKAGE_BASE) :
                              ((Module['filePackagePrefixURL'] || '') + REMOTE_PACKAGE_BASE);
    var REMOTE_PACKAGE_SIZE = 31133632;
    var PACKAGE_UUID = '2325821c-0b97-488e-b09a-1e89c3acbb6b';
  
    function fetchRemotePackage(packageName, packageSize, callback, errback) {
      var xhr = new XMLHttpRequest();
      xhr.open('GET', packageName, true);
      xhr.responseType = 'arraybuffer';
      xhr.onprogress = function(event) {
        var url = packageName;
        var size = packageSize;
        if (event.total) size = event.total;
        if (event.loaded) {
          if (!xhr.addedTotal) {
            xhr.addedTotal = true;
            if (!Module.dataFileDownloads) Module.dataFileDownloads = {};
            Module.dataFileDownloads[url] = {
              loaded: event.loaded,
              total: size
            };
          } else {
            Module.dataFileDownloads[url].loaded = event.loaded;
          }
          var total = 0;
          var loaded = 0;
          var num = 0;
          for (var download in Module.dataFileDownloads) {
          var data = Module.dataFileDownloads[download];
            total += data.total;
            loaded += data.loaded;
            num++;
          }
          total = Math.ceil(total * Module.expectedDataFileDownloads/num);
          if (Module['setStatus']) Module['setStatus']('Downloading data... (' + loaded + '/' + total + ')');
        } else if (!Module.dataFileDownloads) {
          if (Module['setStatus']) Module['setStatus']('Downloading data...');
        }
      };
      xhr.onload = function(event) {
        var packageData = xhr.response;
        callback(packageData);
      };
      xhr.send(null);
    };

    function handleError(error) {
      console.error('package error:', error);
    };
  
      var fetched = null, fetchedCallback = null;
      fetchRemotePackage(REMOTE_PACKAGE_NAME, REMOTE_PACKAGE_SIZE, function(data) {
        if (fetchedCallback) {
          fetchedCallback(data);
          fetchedCallback = null;
        } else {
          fetched = data;
        }
      }, handleError);
    
  function runWithFS() {

function assert(check, msg) {
  if (!check) throw msg + new Error().stack;
}
Module['FS_createPath']('/', 'Resources', true, true);

    function DataRequest(start, end, crunched, audio) {
      this.start = start;
      this.end = end;
      this.crunched = crunched;
      this.audio = audio;
    }
    DataRequest.prototype = {
      requests: {},
      open: function(mode, name) {
        this.name = name;
        this.requests[name] = this;
        Module['addRunDependency']('fp ' + this.name);
      },
      send: function() {},
      onload: function() {
        var byteArray = this.byteArray.subarray(this.start, this.end);

          this.finish(byteArray);

      },
      finish: function(byteArray) {
        var that = this;
        Module['FS_createPreloadedFile'](this.name, null, byteArray, true, true, function() {
          Module['removeRunDependency']('fp ' + that.name);
        }, function() {
          if (that.audio) {
            Module['removeRunDependency']('fp ' + that.name); // workaround for chromium bug 124926 (still no audio with this, but at least we don't hang)
          } else {
            Module.printErr('Preloading file ' + that.name + ' failed');
          }
        }, false, true); // canOwn this data in the filesystem, it is a slide into the heap that will never change
        this.requests[this.name] = null;
      },
    };
      new DataRequest(0, 7692, 0, 0).open('GET', '/level0');
    new DataRequest(7692, 19636, 0, 0).open('GET', '/level1');
    new DataRequest(19636, 56948, 0, 0).open('GET', '/level10');
    new DataRequest(56948, 103220, 0, 0).open('GET', '/level11');
    new DataRequest(103220, 149492, 0, 0).open('GET', '/level12');
    new DataRequest(149492, 195772, 0, 0).open('GET', '/level13');
    new DataRequest(195772, 235068, 0, 0).open('GET', '/level14');
    new DataRequest(235068, 280340, 0, 0).open('GET', '/level15');
    new DataRequest(280340, 350524, 0, 0).open('GET', '/level16');
    new DataRequest(350524, 428668, 0, 0).open('GET', '/level17');
    new DataRequest(428668, 466972, 0, 0).open('GET', '/level18');
    new DataRequest(466972, 509260, 0, 0).open('GET', '/level19');
    new DataRequest(509260, 523684, 0, 0).open('GET', '/level2');
    new DataRequest(523684, 584892, 0, 0).open('GET', '/level20');
    new DataRequest(584892, 592584, 0, 0).open('GET', '/level21');
    new DataRequest(592584, 600276, 0, 0).open('GET', '/level22');
    new DataRequest(600276, 618844, 0, 0).open('GET', '/level3');
    new DataRequest(618844, 642212, 0, 0).open('GET', '/level4');
    new DataRequest(642212, 669556, 0, 0).open('GET', '/level5');
    new DataRequest(669556, 694916, 0, 0).open('GET', '/level6');
    new DataRequest(694916, 721268, 0, 0).open('GET', '/level7');
    new DataRequest(721268, 755588, 0, 0).open('GET', '/level8');
    new DataRequest(755588, 788916, 0, 0).open('GET', '/level9');
    new DataRequest(788916, 914360, 0, 0).open('GET', '/mainData');
    new DataRequest(914360, 916483, 0, 0).open('GET', '/methods_pointedto_by_uievents.xml');
    new DataRequest(916483, 28134179, 0, 0).open('GET', '/sharedassets0.assets');
    new DataRequest(28134179, 28398543, 0, 0).open('GET', '/sharedassets0.resource');
    new DataRequest(28398543, 28402699, 0, 0).open('GET', '/sharedassets1.assets');
    new DataRequest(28402699, 28407911, 0, 0).open('GET', '/sharedassets10.assets');
    new DataRequest(28407911, 28413123, 0, 0).open('GET', '/sharedassets11.assets');
    new DataRequest(28413123, 28418335, 0, 0).open('GET', '/sharedassets12.assets');
    new DataRequest(28418335, 28423547, 0, 0).open('GET', '/sharedassets13.assets');
    new DataRequest(28423547, 28428759, 0, 0).open('GET', '/sharedassets14.assets');
    new DataRequest(28428759, 28433971, 0, 0).open('GET', '/sharedassets15.assets');
    new DataRequest(28433971, 28439183, 0, 0).open('GET', '/sharedassets16.assets');
    new DataRequest(28439183, 28444395, 0, 0).open('GET', '/sharedassets17.assets');
    new DataRequest(28444395, 28449607, 0, 0).open('GET', '/sharedassets18.assets');
    new DataRequest(28449607, 28454819, 0, 0).open('GET', '/sharedassets19.assets');
    new DataRequest(28454819, 28717519, 0, 0).open('GET', '/sharedassets2.assets');
    new DataRequest(28717519, 29002900, 0, 0).open('GET', '/sharedassets2.resource');
    new DataRequest(29002900, 29008112, 0, 0).open('GET', '/sharedassets20.assets');
    new DataRequest(29008112, 29013324, 0, 0).open('GET', '/sharedassets21.assets');
    new DataRequest(29013324, 29017480, 0, 0).open('GET', '/sharedassets22.assets');
    new DataRequest(29017480, 29021636, 0, 0).open('GET', '/sharedassets23.assets');
    new DataRequest(29021636, 29026848, 0, 0).open('GET', '/sharedassets3.assets');
    new DataRequest(29026848, 29032060, 0, 0).open('GET', '/sharedassets4.assets');
    new DataRequest(29032060, 29037272, 0, 0).open('GET', '/sharedassets5.assets');
    new DataRequest(29037272, 29042484, 0, 0).open('GET', '/sharedassets6.assets');
    new DataRequest(29042484, 29047696, 0, 0).open('GET', '/sharedassets7.assets');
    new DataRequest(29047696, 29052908, 0, 0).open('GET', '/sharedassets8.assets');
    new DataRequest(29052908, 29058120, 0, 0).open('GET', '/sharedassets9.assets');
    new DataRequest(29058120, 30633156, 0, 0).open('GET', '/Resources/unity_default_resources');
    new DataRequest(30633156, 31133632, 0, 0).open('GET', '/Resources/unity_builtin_extra');

    function processPackageData(arrayBuffer) {
      Module.finishedDataFileDownloads++;
      assert(arrayBuffer, 'Loading data file failed.');
      var byteArray = new Uint8Array(arrayBuffer);
      var curr;
      
      // Reuse the bytearray from the XHR as the source for file reads.
      DataRequest.prototype.byteArray = byteArray;
          DataRequest.prototype.requests["/level0"].onload();
          DataRequest.prototype.requests["/level1"].onload();
          DataRequest.prototype.requests["/level10"].onload();
          DataRequest.prototype.requests["/level11"].onload();
          DataRequest.prototype.requests["/level12"].onload();
          DataRequest.prototype.requests["/level13"].onload();
          DataRequest.prototype.requests["/level14"].onload();
          DataRequest.prototype.requests["/level15"].onload();
          DataRequest.prototype.requests["/level16"].onload();
          DataRequest.prototype.requests["/level17"].onload();
          DataRequest.prototype.requests["/level18"].onload();
          DataRequest.prototype.requests["/level19"].onload();
          DataRequest.prototype.requests["/level2"].onload();
          DataRequest.prototype.requests["/level20"].onload();
          DataRequest.prototype.requests["/level21"].onload();
          DataRequest.prototype.requests["/level22"].onload();
          DataRequest.prototype.requests["/level3"].onload();
          DataRequest.prototype.requests["/level4"].onload();
          DataRequest.prototype.requests["/level5"].onload();
          DataRequest.prototype.requests["/level6"].onload();
          DataRequest.prototype.requests["/level7"].onload();
          DataRequest.prototype.requests["/level8"].onload();
          DataRequest.prototype.requests["/level9"].onload();
          DataRequest.prototype.requests["/mainData"].onload();
          DataRequest.prototype.requests["/methods_pointedto_by_uievents.xml"].onload();
          DataRequest.prototype.requests["/sharedassets0.assets"].onload();
          DataRequest.prototype.requests["/sharedassets0.resource"].onload();
          DataRequest.prototype.requests["/sharedassets1.assets"].onload();
          DataRequest.prototype.requests["/sharedassets10.assets"].onload();
          DataRequest.prototype.requests["/sharedassets11.assets"].onload();
          DataRequest.prototype.requests["/sharedassets12.assets"].onload();
          DataRequest.prototype.requests["/sharedassets13.assets"].onload();
          DataRequest.prototype.requests["/sharedassets14.assets"].onload();
          DataRequest.prototype.requests["/sharedassets15.assets"].onload();
          DataRequest.prototype.requests["/sharedassets16.assets"].onload();
          DataRequest.prototype.requests["/sharedassets17.assets"].onload();
          DataRequest.prototype.requests["/sharedassets18.assets"].onload();
          DataRequest.prototype.requests["/sharedassets19.assets"].onload();
          DataRequest.prototype.requests["/sharedassets2.assets"].onload();
          DataRequest.prototype.requests["/sharedassets2.resource"].onload();
          DataRequest.prototype.requests["/sharedassets20.assets"].onload();
          DataRequest.prototype.requests["/sharedassets21.assets"].onload();
          DataRequest.prototype.requests["/sharedassets22.assets"].onload();
          DataRequest.prototype.requests["/sharedassets23.assets"].onload();
          DataRequest.prototype.requests["/sharedassets3.assets"].onload();
          DataRequest.prototype.requests["/sharedassets4.assets"].onload();
          DataRequest.prototype.requests["/sharedassets5.assets"].onload();
          DataRequest.prototype.requests["/sharedassets6.assets"].onload();
          DataRequest.prototype.requests["/sharedassets7.assets"].onload();
          DataRequest.prototype.requests["/sharedassets8.assets"].onload();
          DataRequest.prototype.requests["/sharedassets9.assets"].onload();
          DataRequest.prototype.requests["/Resources/unity_default_resources"].onload();
          DataRequest.prototype.requests["/Resources/unity_builtin_extra"].onload();
          Module['removeRunDependency']('datafile_WebGL.data');

    };
    Module['addRunDependency']('datafile_WebGL.data');
  
    if (!Module.preloadResults) Module.preloadResults = {};
  
      Module.preloadResults[PACKAGE_NAME] = {fromCache: false};
      if (fetched) {
        processPackageData(fetched);
        fetched = null;
      } else {
        fetchedCallback = processPackageData;
      }
    
  }
  if (Module['calledRun']) {
    runWithFS();
  } else {
    if (!Module['preRun']) Module['preRun'] = [];
    Module["preRun"].push(runWithFS); // FS is not initialized yet, wait for it
  }

})();
