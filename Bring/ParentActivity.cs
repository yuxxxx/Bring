
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Bring
{
    [Activity(Label = "ParentActivity")]
    public class ParentActivity : Activity
    {
        ListView NotesView => FindViewById<ListView>(Resource.Id.ParentNotesListView);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Parent);
            Bring.ParentStatus.Reset();

            var adapter = new NoteAdapter(this, 0, Bring.Items, Bring.ParentStatus);
            adapter.Switched += OnSwitched;
            Title = "0";

            NotesView.Adapter = adapter;
        }

        void OnSwitched(object sender, EventArgs e)
        {
            Title = Bring.ParentStatus.Reverse().Aggregate(0u, (a, b) => a = ((a << 1) | (b ? 1u : 0u))).ToString();
        }
    }
}
