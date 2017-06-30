using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using HackAtHome.CustomAdapters;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName")]
    public class EvidenceActivity : BaseActivity
    {
        ListEvidenceFragment Data;
        string Token;
        string UserName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Evidences);
            Token = Intent.GetStringExtra("Token");
            SetUserName();
            FillEvidenceList();
        }

        void SetUserName()
        {
            UserName = Intent.GetStringExtra("User");
            var user = FindViewById<TextView>(Resource.Id.textViewUser);
            user.Text = UserName;
        }

        async void FillEvidenceList()
        {
            Data = (ListEvidenceFragment)FragmentManager.FindFragmentByTag("Evidences");

            if (Data == null)
            {
                Data = new ListEvidenceFragment();
                var fragmentTransaction = FragmentManager.BeginTransaction();
                fragmentTransaction.Add(Data, "Evidences");

                Data.Evidences = await Helper.GetEvidences(Token);
                fragmentTransaction.Commit();
            }

            var listEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);

            listEvidences.Adapter = new EvidencesAdapter(this, Data.Evidences, Resource.Layout.ListEvidences, Resource.Id.textViewEvidenceName, Resource.Id.textViewEvidenceStatus);

            listEvidences.ItemClick += ListEvidences_ItemClick;
        }

        void ListEvidences_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var id = Convert.ToInt32(e.Id);
            var activity = new Intent(this, typeof(EvidenceDetailActivity));
            var evidence = Data.Evidences.SingleOrDefault(x => x.EvidenceID == id);

            activity.PutExtra("EvidenceTitle", evidence?.Title);
            activity.PutExtra("EvidenceStatus", evidence?.Status);
            activity.PutExtra("User", UserName);
            activity.PutExtra("Token", Token);
            activity.PutExtra("EvidenceId", id);

            StartActivity(activity);
        }
    }
}