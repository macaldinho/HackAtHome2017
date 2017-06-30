using System.Collections.Generic;
using Android.App;
using Android.OS;
using HackAtHome.Entities;

namespace HackAtHome
{
    public class ListEvidenceFragment : Fragment
    {
        public List<Evidence> Evidences { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
    }
}