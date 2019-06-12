
exports.HashTable = function () {
  var _size = 0;
  var _hash = new Object();

  this.add = function (key, value) {
	if (key){
    if (!this.containsKey(key)) {
      ++_size;
    }
    _hash[key] = value;
	}
  }

  this.remove = function (key) {
    if (key){
    if (this.containsKey(key)) {
      if (delete _hash[key]) {
        --_size;
        }
      }
    }
  }

  this.containsKey = function (key) {
    if (key){
    return (key in _hash);
	}
	return false;
  }

  this.getValue = function (key) {
    if (key){
    if (this.containsKey(key)) {
      return _hash[key];
    }
	}
    return null;
  }

  this.getSize = function () {
    return _size;
  }

  this.visit = function (cb) {
    for (var key in _hash) {
      cb(key, _hash[key]);
    }
  }
}