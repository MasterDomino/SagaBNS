using SagaBNS.DTO;
using SagaBNS.Entity;

namespace SagaBNS.Mapper
{
    public static class TeleportLocationMapper
    {
        #region Methods

        public static bool ToTeleportLocation(TeleportLocationDTO input, TeleportLocation output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.TeleportId = input.TeleportId;
            return true;
        }

        public static bool ToTeleportLocationDTO(TeleportLocation input, TeleportLocationDTO output)
        {
            if (input == null)
            {
                output = null;
                return false;
            }
            output.CharacterId = input.CharacterId;
            output.TeleportId = input.TeleportId;
            return true;
        }

        #endregion
    }
}