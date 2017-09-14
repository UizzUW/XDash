using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;

namespace XDash.Framework.Droid.FilePicker
{
    public enum PickerType
    {
        File,
        Files,
        Folder
    }

    /// <summary>
    /// Implementation for Feature
    /// </summary>
    ///
    [Preserve(AllMembers = true)]
    public class FilePickerImplementation
    {
        private readonly Context _context;
        private int _requestId;
        private TaskCompletionSource<object> _completionSource;

        public FilePickerImplementation()
        {
            _context = Application.Context;
        }

        public async Task<string> PickFile()
        {
            return (string)await TakeMediaAsync(PickerType.File);
        }

        public async Task<List<string>> PickFiles()
        {
            var result = await TakeMediaAsync(PickerType.Files);
            if (result is string s)
            {
                return new List<string> { s };
            }
            return result as List<string>;
        }

        public async Task<string> PickFolder()
        {
            return (string)await TakeMediaAsync(PickerType.Folder);
        }

        private async Task<object> TakeMediaAsync(PickerType pickerType)
        {
            var id = GetRequestId();

            var ntcs = new TaskCompletionSource<object>(id);

            if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at a time");

            try
            {
                var pickerIntent = new Intent(_context, typeof(FilePickerActivity));
                pickerIntent.PutExtra("pickerType", (int)pickerType);
                pickerIntent.SetFlags(ActivityFlags.NewTask);

                _context.StartActivity(pickerIntent);

                EventHandler<object> handler = null;
                EventHandler<EventArgs> cancelledHandler = null;

                handler = (s, e) =>
                {
                    var tcs = Interlocked.Exchange(ref _completionSource, null);

                    FilePickerActivity.FilePicked -= handler;

                    tcs?.SetResult(e);
                };

                cancelledHandler = (s, e) =>
                {
                    var tcs = Interlocked.Exchange(ref _completionSource, null);

                    FilePickerActivity.FilePickCancelled -= cancelledHandler;

                    tcs?.SetResult(null);
                };

                FilePickerActivity.FilePickCancelled += cancelledHandler;
                FilePickerActivity.FilePicked += handler;
            }
            catch (Exception exAct)
            {
                Debug.Write(exAct);
            }

            return await _completionSource.Task;
        }

        private int GetRequestId()
        {
            int id = _requestId;

            if (_requestId == int.MaxValue)
                _requestId = 0;
            else
                _requestId++;

            return id;
        }
    }
}