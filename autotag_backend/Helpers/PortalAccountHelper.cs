using System;
using AutoTagBackEnd.Entities;
using AutoTagBackEnd.Models;

namespace AutoTagBackEnd.Helpers
{
	public static class PortalAccountHelper
	{
		public static Entities.PortalAccountStatus GetStatus(PortalAccount portalAccount)
        {
			List<PortalAccountStatus> listPortalAccountStatus = new List<PortalAccountStatus>()
			{
				new PortalAccountStatus("active", "Activo", "Descargado correctamente"),
				new PortalAccountStatus("error", "Con error", "Error en descarga"),
				new PortalAccountStatus("error_login", "Error en credenciales", "Credenciales erroneas"),
				new PortalAccountStatus("processing", "Procesando", "Se están descargando datos desde el portal"),
				new PortalAccountStatus("disabled", "Deshabilitado", "Deshabilitado")
			};

			if(!portalAccount.Enabled)
            {
				return listPortalAccountStatus.Find(p => p.Code == "disabled");
            }

			if(portalAccount.IsBeingProcessed)
            {
				return listPortalAccountStatus.Find(p => p.Code == "processing");
			}

			if(portalAccount.HasError && portalAccount.HasLoginError)
            {
				return listPortalAccountStatus.Find(p => p.Code == "error_login");
			}

			if (portalAccount.HasError)
			{
				return listPortalAccountStatus.Find(p => p.Code == "error");
			}

			return listPortalAccountStatus.Find(p => p.Code == "active");
		}
	}
}

