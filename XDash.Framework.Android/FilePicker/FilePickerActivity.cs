using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;

namespace XDash.Framework.Droid.FilePicker
{
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    [Preserve(AllMembers = true)]
    public class FilePickerActivity : Activity
    {
        private Context context;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            context = Application.Context;

            var pickerType = (PickerType)Intent.GetIntExtra("pickerType", 0);

            Intent intent = null;
            if (pickerType == PickerType.Files)
            {
                intent = new Intent(Intent.ActionGetContent);
                intent.SetType("*/*");
                intent.PutExtra(Intent.ExtraAllowMultiple, true);
            }
            else if (pickerType == PickerType.Folder)
            {
                intent = new Intent(Intent.ActionOpenDocumentTree);
                //intent.SetData(Android.Net.Uri.Parse(Android.OS.Environment.ExternalStorageDirectory.Path));
                intent.SetType("*/*");
            }

            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "Select file"), 0);
            }
            catch (Exception exAct)
            {
                System.Diagnostics.Debug.Write(exAct);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Canceled)
            {
                // Notify user file picking was cancelled.
                OnFilePickCancelled();
                Finish();
            }
            else
            {
                System.Diagnostics.Debug.Write(data.Data);
                try
                {
                    var _uri = data.Data;

                    var filePath = IOUtil.getPath(context, _uri);

                    if (string.IsNullOrEmpty(filePath))
                        filePath = _uri.Path;

                    var fileName = GetFileName(context, _uri);

                    OnFilePicked(filePath);
                }
                catch (Exception readEx)
                {
                    // Notify user file picking failed.
                    OnFilePickCancelled();
                    System.Diagnostics.Debug.Write(readEx);
                }
                finally
                {
                    Finish();
                }
            }
        }

        string GetFileName(Context ctx, Android.Net.Uri uri)
        {

            string[] projection = { MediaStore.MediaColumns.DisplayName };

            var cr = ctx.ContentResolver;
            var name = "";
            var metaCursor = cr.Query(uri, projection, null, null, null);

            if (metaCursor != null)
            {
                try
                {
                    if (metaCursor.MoveToFirst())
                    {
                        name = metaCursor.GetString(0);
                    }
                }
                finally
                {
                    metaCursor.Close();
                }
            }
            return name;
        }

        internal static event EventHandler<object> FilePicked;
        internal static event EventHandler<EventArgs> FilePickCancelled;

        private static void OnFilePickCancelled()
        {
            FilePickCancelled?.Invoke(null, null);
        }

        private static void OnFilePicked(object e)
        {
            var picked = FilePicked;

            if (picked != null)
                picked(null, e);
        }
    }
}