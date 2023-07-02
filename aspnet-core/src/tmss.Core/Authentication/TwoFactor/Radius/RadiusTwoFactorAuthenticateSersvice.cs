using Abp.Dependency;
using Abp.UI;
using FP.Radius;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Authentication.TwoFactor.Radius
{
    public class RadiusTwoFactorAuthenticateSersvice : tmssServiceBase, ITransientDependency
    {

        public RadiusTwoFactorAuthenticateSersvice()
        {

        }

        public async Task<string> SendCodeAsync(string userName, string password)
        {
            RadiusClient rc = new RadiusClient(tmssConsts.RADIUS_SERVER_NAME, tmssConsts.RADIUS_SHARED_SECRET, 3000, tmssConsts.RADIUS_PORT);
            RadiusPacket authPacket = rc.Authenticate(userName, password);
            authPacket.SetAttribute(new VendorSpecificAttribute(10135, 1, UTF8Encoding.UTF8.GetBytes("Testing")));
            authPacket.SetAttribute(new VendorSpecificAttribute(10135, 2, new[] { (byte)7 }));
            RadiusPacket receivedPacket = await rc.SendAndReceivePacket(authPacket);
            if (receivedPacket == null)
            {
                throw new UserFriendlyException(L("SendSecurityCodeErrorMessage"));
            }
            else if (receivedPacket.PacketType == RadiusCode.ACCESS_CHALLENGE)
            {
                //ViewBag.DataState = Encoding.Default.GetString(receivedPacket.Attributes[1].Data);
                return Encoding.Default.GetString(receivedPacket.Attributes[1].Data);
            }
            else
            {
                throw new UserFriendlyException(L("SendSecurityCodeErrorMessage"));
            }
        }

        public async Task<bool> ValidateCodeAsync(string userName, string code, string dataState)
        {
            //string userName = HttpContext.Session.GetString("s_user_name");
            RadiusClient rcVerify = new RadiusClient(tmssConsts.RADIUS_SERVER_NAME, tmssConsts.RADIUS_SHARED_SECRET, 3000, tmssConsts.RADIUS_PORT);
            RadiusPacket authPacketVerify = rcVerify.Authenticate(userName, code);
            authPacketVerify.SetAttribute(new RadiusAttribute(RadiusAttributeType.STATE, Encoding.Default.GetBytes(dataState)));
            RadiusPacket receivedPacket = await rcVerify.SendAndReceivePacket(authPacketVerify);
            if (receivedPacket == null)
            {
                return true;
            }
            else if (receivedPacket.PacketType == RadiusCode.ACCESS_REJECT)
            {
                return true;
            }
            else if (receivedPacket.PacketType == RadiusCode.ACCESS_ACCEPT)
            {
                return false;
            } else
            {
                return true;
            }
             
        }
    }
}
