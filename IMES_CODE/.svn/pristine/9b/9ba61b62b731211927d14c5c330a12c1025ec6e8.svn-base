var IMESUti = IMESUti || {};
IMESUti.Queue = (function() {
    var stack = [];
    return {
        Enqueue: function(value) {
            stack.push(value);
        },
        Dequeue: function() {
            return stack.shift();
        },
        Count: function() {
            return stack.length;
        },
        Clear: function() {
            if (stack.length > 0) {
                stack = [];
            }
        },
        Peek: function() {
            if (stack.length > 0) {
                return stack[0];
            } else {
                return null;
            }
        }
    };
})();