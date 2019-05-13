using UnityEngine;
using System.Collections;

namespace CharControl
{
	public class input_keylistener
	{
		static input_keylistener() {}
	    private int _code = -1;

        private static input_keylistener _listener;
        public static input_keylistener GetListener()
	    {
	        if (_listener == null)
                _listener = new input_keylistener();
	        return _listener;
	    }

	    public void RecordInputKey(int code)
	    {
	        _code = code;
	    }

	    public int RetrieveInputKey()
	    {
	        return _code;
	    }
	}
}