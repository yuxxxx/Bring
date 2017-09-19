using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using Java.Lang;
using System;
using Android.Content.Res;
using System.Threading.Tasks;

namespace Bring
{
    [Activity(Label = "Bring", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        ListView NotesView => FindViewById<ListView>(Resource.Id.MainNotesListView);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Title = "お使いリスト";

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var adapter = new NoteAdapter(this, 0, Bring.Items, Bring.MainStatus);
            NotesView.Adapter = adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return Select(item.ItemId, this);
        }

        public bool Select(int itemId, Context context)
        {
            switch (itemId)
            {
                case Resource.Id.inputNumber:
                    var result = InputNumber();
                    Apply(result);
                    return true;
                case Resource.Id.toParent:
                    GoToParentalActivity();
                    return true;
            }

            return false;
        }

        Task<string> InputNumber()
        {
            var tcs = new TaskCompletionSource<string>();

            var editText = new EditText(this);
            editText.Text = "";

            new AlertDialog.Builder(this)
                           .SetTitle("親に言われた数字を入力")
                           .SetView(editText)
                           .SetNegativeButton("キャンセル", (o, e) => tcs.SetResult(""))
                           .SetPositiveButton("OK", (o, e) => tcs.SetResult(editText.Text))
                           .Show();

            return tcs.Task;
        }

        async void Apply(Task<string> result)
        {
            var r = await result;

            var items = Bring.GetItems(uint.Parse(r));
            NotesView.Adapter = new NoteAdapter(this, 0, items.ToList(), Enumerable.Repeat(false, items.Count()).ToList());
            NotesView.RefreshDrawableState();
        }

        void GoToParentalActivity()
        {
            var intent = new Intent(this, typeof(ParentActivity));
            StartActivity(intent);
        }

    }

    public class NoteAdapter : ArrayAdapter<string>
    {
        private LayoutInflater LayoutInflater;
        private IList<bool> Status;

        public event EventHandler Switched;

        public NoteAdapter(Context c, int id, IEnumerable<string> notes, IList<bool> status) : base(c, id, notes.ToArray())
        {
            Status = status;
            LayoutInflater = c.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.Inflate(
                        Resource.Layout.SwitchCell,
                        parent,
                        false
                );
            }

            var note = GetItem(position);

            var itemSwitch = convertView.FindViewById(Resource.Id.NoteSwitch) as Switch;
            itemSwitch.Checked = Status.ElementAt(position);
            itemSwitch.Text = note;
            itemSwitch.Tag = position;

            itemSwitch.Click += Clicked;

            return convertView;
        }

        protected virtual void Clicked (object sender, EventArgs e)
        {
            var sw = sender as Switch;
            var index = (int)sw.Tag;
            Status[index] = sw.Checked;
            Switched?.Invoke(this, e);
        }

    }

    public class Note
    {
        public string Title { get; private set; }
        public bool Checked { get; set; }

        public Note() { }
        public Note(string title, bool @checked)
        {
            Title = title;
            Checked = @checked;
        }
    }
}
