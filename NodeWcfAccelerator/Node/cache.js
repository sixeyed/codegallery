/**
* A simple cache for key value pairs.
* Options are:
*   capacity: default 512 entries
*   replacement: default 'smart', 'lfu'
*   maxFrequency: freqeuncy saturation for 'lfu', default 16
*
* LFU replacement:
* LFU stands for least freqeuntly used. If multiple canidates are LFU then the oldest cache entry
* is removed. The usage frequency of an entry saturates, the default maxmimum frequency is 16.
*
* SMART replacement:
* Oldest cache entry with the lowest smart value is replaced. The start smart value
* is cache capacity / 2. On a cache hit the entry's smart value is set to cache capacity.
* On a cache miss all entries' smart value is decreased by one but not less than zero.
* (If you know the proper name of this cache algorithm, please drop me an email.)
*
* Write strategy:
* You have to enforce the write strategy yourself. The cached object can write-through every change
* or you can write-back the object when it is replaced. For a write-back strategy the .add() method
* returns the replaced entry if any.
*
* See the basic usage in the unit tests. On a cache miss you have to retrieve
* the value for your data source and store it in the cache with an appropiate key.
* If a cache entry with the same key already exists, the old entry is replaced.
*
* @author Kim root@aggressive-solutions.de
* @version 0.3
*/

var assert = require('assert');

/**
* Creates a new Cache.
* @constructor
*/
exports.Cache = function () {
    var buffer = {};
    var hits = 0;
    var miss = 0;

    /**
    * Default options
    * @private
    */
    var defaultOptions = {
        capacity: 512,
        maxFrequency: 16,
        replacement: 'smart'
    };
    var options = {};

    /**
    * Apply options.
    * @private
    */
    function setOptions(opts) {
        // set the options provided or keep default.
        var o;
        for (o in opts) {
            if (opts.hasOwnProperty(o)) {
                options[o] = opts[o];
            }
        }

        assert.ok(options.capacity > 0, 'Cache capacity must be greater than 0.');
        assert.ok(options.maxFrequency > 0, 'Cache max freqeuncy must be greater than 0.');
    }
    setOptions(defaultOptions);

    /**
    * Get internal buffer size
    * @private
    * @returns {integer} size
    */
    function getBufferSize() {
        var size = 0, key;
        for (key in buffer) {
            if (buffer.hasOwnProperty(key)) {
                size++;
            }
        }
        return size;
    };

    /**
    * Remove the least frequently used entry and return it.
    * @private
    * @returns The removed entry
    */
    function removeLfu() {
        var size = 0,
            key,
            minFreq = options.maxFrequency,
            candidate;

        for (key in buffer) {
            if (buffer.hasOwnProperty(key)) {
                var entry = buffer[key];

                // TRICKY: oldest entry is the first and
                // also default remove candidate.
                if (entry.reads < minFreq || !candidate) {
                    // console.log('New LFU key: ' + key + ' reads: ' + entry.reads);
                    candidate = key;
                    minFreq = entry.reads;
                }
            }
        }

        var value;
        if (candidate) {
            value = buffer[candidate].value;
            // console.log('Deleting key: ' + candidate);
            delete buffer[candidate];
        }
        return value;
    }

    /**
    * Remove the recently least useful or oldest entry and return it.
    * @private
    * @returns The removed value.
    */
    function removeSmart() {
        var key;
        var candidate;
        var minUseful = options.capacity;

        for (key in buffer) {
            if (buffer.hasOwnProperty(key)) {
                var entry = buffer[key];
                // TRICKY: First entry is also the oldest.
                // console.log('key: ' + key.toString() + ' smart: ' + entry.smart);
                if (entry.smart < minUseful || !candidate) {
                    minUseful = entry.smart;
                    candidate = key;
                }
            }
        }

        var value;
        if (candidate) {
            value = buffer[candidate].value;
            // console.log('SMART deleting key: ' + candidate);
            delete buffer[candidate];
        }

        return value;
    }

    /**
    * Get the current cache size.
    * @public
    * @return {integer} Size
    */
    this.size = getBufferSize;

    /**
    * Initialize cache. Calling it again resets cache and its statistics.
    * Missing options will set default cache options.
    * @public
    * @param {Object} opt Options capacity and maxFrequency (LFU limit)
    */
    this.init = function (opts) {
        buffer = {};
        hits = 0;
        miss = 0;

        if (!opts) {
            setOptions(defaultOptions);
            return;
        }

        setOptions(opts);
    }

    /**
    * Add key value pair to cache. Returns the relpaced entry if any. This
    * is required by some write strategies.
    * @public
    * @param key Must be a string or have a .toString() method.
    * @param value The actual value to cache.
    * @returns The replaced entry if any, null otherwise.
    */
    this.add = function (key, value) {

        assert.ok(typeof key.toString === 'function', 'Key has no .toString()');

        if (!value) {
            value = key;
        }

        var entry = {
            value: value,
            reads: 0,   // access frequency
            smart: options.capacity / 2    // smart value of entry (higher is more useful)
        };

        var old;
        if (getBufferSize() >= options.capacity) {
            // console.log('cache is full.');
            if (options.replacement === 'smart') {
                old = removeSmart();
            } else {
                old = removeLfu();
            }
        }

        // console.log('Adding key: ' + key + ' value: ' + value);
        buffer[key.toString()] = entry;

        return old;
    };

    /**
    * Handle cache miss
    * @private
    */
    function handleMiss() {
        miss += 1;
        if (options.replacement === 'smart') {
            var key;
            for (key in buffer) {
                if (buffer.hasOwnProperty(key)) {
                    // decrease smart value of all entries because
                    // they were not useful.
                    var entry = buffer[key];
                    entry.smart -= 1;
                    entry.smart = Math.max(entry.smart, 0);
                }
                else {

                }
            }
        }
        else {
            // lfu
            // NOTHING TODO HERE!
        }
    }

    /*
    * handle cache hit
    * @private
    */
    function handleHit(entry) {
        hits += 1;
        if (options.replacement === 'smart') {
            // entry was useful, set highest value
            entry.smart = options.capacity;
        }
        else {
            entry.reads += 1;
            entry.reads = Math.min(entry.reads, options.maxFrequency);
        }
    }

    /**
    * Get the value for the given key.
    * @public
    * @param key Must be string or have a toString() method.
    * @returns The value if the key exists, null otherwise.
    */
    this.get = function (key) {

        var entry = buffer[key.toString()];
        if (!entry) {
            handleMiss();
            // console.log('key: ' + key + ' MISS!');
            return null;
        }

        handleHit(entry);
        // console.log('key: ' + key + ' HIT! reads: ' + entry.reads);
        return entry.value;
    };

    /**
    * Get cache statatistics
    * @public
    */
    this.getStatistics = function () {
        var stats = {
            hits: hits,
            miss: miss,
            hit_rate: hits / (hits + miss),
            hit_ratio: hits / miss,
            capacity: options.capacity,
            size: getBufferSize(),
            maxFrequency: options.maxFrequency,
            replacement: options.replacement
        };

        return stats;
    };

    this.logContent = function () {
        var key,
            counter = 0;

        console.log('--- Cache ---');
        for (key in buffer) {
            if (buffer.hasOwnProperty(key)) {
                var entry = buffer[key.toString()];
                console.log(counter + '. key: ' + key + ' value: ' +
                    entry.value + ' reads: ' + entry.reads + ' smart: ' +
                    entry.smart);
                counter += 1;
            }
        }
    };
};

/*
* ----------------------------------------------------------------------------
* "THE BEER-WARE LICENSE" (Revision 42):
* <root@aggressive-solutions.de> wrote this file. As long as you retain this
* notice you can do whatever you want with this stuff. If we meet some day,
* and you think this stuff is worth it, you can buy me a beer in return. KIM
* ----------------------------------------------------------------------------
*/

