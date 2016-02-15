/* jQuery exclusion in/out | He Jiang (hejiang@tju.edu.cn) */
(function($, w, undefined) {
	$.exclusionInOut = function(options, data) {
		var _t = $.type(options);
		
		// initialize
		if (_t == 'undefined' || _t == 'object') {
			var _opt = $.extend({ timeout: 500 }, options);
			var _to = _opt.timeout;
			var _fa = _opt.acquire;
			var _fr = _opt.release;

			var _got_resource = false;
			var _acquire_pending = false;
			var _release_pending = false;

			var _func_acquire = function() {
				if (!_got_resource && _acquire_pending) {
					if (_fa()) {
						_got_resource = true;
						_acquire_pending = false;
						_release_pending = false;
						return;
					}
					w.setTimeout(_func_acquire, _to);
				}
			};
			var _func_release = function() {
				if (_got_resource && _release_pending) {
					if (_fr()) {
						_got_resource = false;
						_acquire_pending = false;
						_release_pending = false;
						return;
					}
					w.setTimeout(_func_release, _to);
				}
			};
			$('html').focusin(function() {
				if (_got_resource) {
					_acquire_pending = false;
					_release_pending = false;
				}
				else {
					_acquire_pending = true;
					_release_pending = false;
					
					w.setTimeout(_func_acquire, _to);
				}
			}).focusout(function() {
				if (_got_resource) {
					_acquire_pending = false;
					_release_pending = true;
					
					w.setTimeout(_func_release, _to);
				}
				else {
					_acquire_pending = false;
					_release_pending = false;
				}
			}).focus();
		}
	};
})(jQuery, window);
