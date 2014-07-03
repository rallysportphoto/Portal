/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Helpers
{
    public class AsyncHelper
    {
        public static void TransferCompletion<T,T1>(
            TaskCompletionSource<T> tcs, T1 e,  Func<T> getResult) where T1: System.ComponentModel.AsyncCompletedEventArgs 
        {
            if (e.Error != null)
            {
                tcs.TrySetException(e.Error);
            }
            else if (e.Cancelled)
            {
                tcs.TrySetCanceled();
                
            }
            else
            {
                tcs.TrySetResult(getResult());
            }
        }        
    }    
}