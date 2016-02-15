/* jQuery barcode inputbox | He Jiang (hejiang@tju.edu.cn) */
(function($, w, undefined) {
	$.fn.expressInput = function(options, data) {
		return this.each(function(){
			var _t = $.type(options);
			
			// initialize
			if (_t == 'undefined' || _t == 'object') {
				var _opt = $.extend({ timeout: 200, filter: '\\W', transform: 'uppercase', queue: [] }, options);
				var _th = this;
				var _to = _opt.timeout;
				var _q = _opt.queue;
				var _ft = _opt.filter;
				var _tf = _opt.transform;
				var _to_f = function() {
					var _cb = _opt.callback;
					if (_q.length && _cb) {
						var _v = _q.shift();
						if (_v && _ft) {
							var _re = new RegExp(_ft, 'g');
							_v = _v.replace(_re, '');
						}
						if (_v && _tf) {
							_tf = _tf.toLowerCase();
							if (_tf == 'uppercase')
								_v = _v.toUpperCase();
							else if (_tf == 'lowercase')
								_v = _v.toLowerCase();
						}
						if (_v) {
							delete _opt.callback;
							_cb.call(_th, _v);
						}
					}
					w.setTimeout(_to_f, _to);
				};
				
				$(this).keydown(function(e) {
					if (e.which == 13 || e.which == 9) {
						e.preventDefault();
						_q.push(this.value);
						this.value = '';
					}
				}).prop('options', _opt)
				//.css({ 'ime-mode': 'disabled' });

				w.setTimeout(_to_f, _to);
				this.focus();
			}
			
			// set callback
			if (options == 'setCallback') {
				$(this).prop('options').callback = data;
			}
		});
	};
})(jQuery, window);
