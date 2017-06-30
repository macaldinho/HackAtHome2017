using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HackAtHome.Entities;
using HackAtHome.SAL;

namespace HackAtHome
{
    public class Helper
    {
        static readonly ServiceClient Service;
        static readonly MicrosoftServiceClient MicrosoftService;

        static Helper()
        {
            Service = new ServiceClient();
            MicrosoftService = new MicrosoftServiceClient();
        }

        public async Task<(string message, ResultInfo result)> Authenticate(string email, string password, string deviceId)
        {
            var message = string.Empty;

            var result = await Service.AutenticateAsync(email, password);

            if (result.Status == Status.Success)
            {
                var labItem = new LabItem
                {
                    Email = email,
                    Lab = "Hack@Home",
                    DeviceId = deviceId,
                };

                await MicrosoftService.SendEvidence(labItem);

                if (string.IsNullOrWhiteSpace(labItem.Id))
                {
                    message = "Error - Microsoft";
                }
            }
            else
            {
                message = $"Error - TICapacitacion {result.Status}";
            }
            return (message, result);
        }

        public async Task<List<Evidence>> GetEvidences(string token)
        {
            return await Service.GetEvidencesAsync(token);
        }

        public async Task<EvidenceDetail> GetEvidenceById(string token, int id)
        {
            return await Service.GetEvidenceByIDAsync(token, id);
        }
    }
}