using AntDesign;
using BusinessWeb.Data;
using BusinessWeb.Models;
using BusinessWeb.Models.BusinessWebDB;
using BusinessWeb.Models.DB;
using BusinessWeb.Models.LT;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using Newtonsoft.Json.Linq;
using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;


namespace BusinessWeb.Controllers.SAGE_Save
{
    [Route("api/[controller]")]
    [ApiController]


    public partial class Save_DOCENTETEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public Save_DOCENTETEController()
        {

        }
        [HttpPost("create")]
        public IActionResult CreateEntete([FromBody] SageData data)
        {
            try
            {
                return Ok(new { result = CreateDocumentVente(data.DocEntete) });
            }
            catch (Exception ex)
            {
                return Ok(new { result = StatusCode(500, ex.Message) });
            }
        }
        private DocumentType GetDocumentType(short? type)
        {
                       switch (type)
            {
                case 0:
                    return DocumentType.DocumentTypeVenteDevis;
                case 1:
                    return DocumentType.DocumentTypeVenteCommande;
                case 2:
                    return DocumentType.DocumentTypeVentePrepaLivraison;
                case 3:
                    return DocumentType.DocumentTypeVenteLivraison;
                default:
                    throw new ArgumentException("Type de document inconnu");
            }
        }
        private string CreateDocumentVente(API_LT_DOCENTETE docEntete)
        {
             SageOM sageOM = new SageOM();
            var oCial = sageOM.CIAL();
            var oCpta = sageOM.CPTA();
            try
            {
                IPMDocument mProcessDoc = oCial.CreateProcess_Document(GetDocumentType(docEntete.DO_Type));

                // Conversion du document du processus dans le type du document de destination : Preparation de commande
                IBODocumentVente3 mDoc = (IBODocumentVente3)mProcessDoc.Document;

                // Désactivation du recalcul automatique des totaux
                mDoc.SetAutoRecalculTotaux(false);

                // Affectation du client au document
                mDoc.SetDefaultClient(oCial.CptaApplication.FactoryClient.ReadNumero(docEntete.DO_Tiers));
                if (!String.IsNullOrEmpty(docEntete.CA_Num))
                {
                    mDoc.CompteA = oCpta.FactoryCompteA.ReadNumero(oCpta.FactoryAnalytique.ReadIntitule("AFFAIRES"), docEntete.CA_Num);
                }

                if (!mProcessDoc.CanProcess)
                {
                    return GetErrors(mProcessDoc);
                }

                mProcessDoc.Process();
                //return "Document créé avec succès.";
                return mDoc.DO_Piece;
            }
            catch (Exception ex)
            {
                return "Erreur : " + ex.Message;
            }
            finally
            {
                if (oCial != null) CloseBase(oCial);
                if (oCpta != null) CloseBaseCpta(oCpta);
            }
        }
        private string GetErrors(IPMProcess proc)
        {
            string result = "Erreurs :";
            for (int i = 1; i <= proc.Errors.Count; i++)
            {
                var err = proc.Errors[i];
                result += $"\nCode : {err.ErrorCode}, Indice : {err.Indice}, Description : {err.Text}";
            }
            return result;
        }
        public static bool CloseBase(BSCIALApplication100c BaseCial)
        {
            try
            {
                if (BaseCial.IsOpen) BaseCial.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Erreur lors de la fermeture de la base : " + ex.Message);
                return false;
            }
        }

        public static bool CloseBaseCpta(BSCPTAApplication100c BaseCpta)
        {
            try
            {
                if (BaseCpta.IsOpen) BaseCpta.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Erreur lors de la fermeture de la base : " + ex.Message);
                return false;
            }
        }



    }
}