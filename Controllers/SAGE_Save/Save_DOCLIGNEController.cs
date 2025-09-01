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
using System.Text.Json;
using System.Threading.Tasks;
using static BusinessWeb.Controllers.SAGE_Save.Save_DOCLIGNEController;


namespace BusinessWeb.Controllers.SAGE_Save
{
    [Route("api/[controller]")]
    [ApiController]


    public partial class Save_DOCLIGNEController : ControllerBase
    {
        public DB _db = new DB(); BusinessWebDBContext _sdb = new BusinessWebDBContext();  Helpers fn = new Helpers();

        public Save_DOCLIGNEController()
        {

        }
        [HttpPost("create")]
        public IActionResult CreateLigne([FromBody] SageData data)
        {
            try
            {
                return Ok(new { result = CreateDocumentVenteLigne(data.DocEntete,data.DocLignes) });
            }
            catch (Exception ex)
            {
                return Ok(new { result = JsonSerializer.Serialize(new AR_Result() { Erreur = ex.Message }) });
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
        private string CreateDocumentVenteLigne(API_LT_DOCENTETE docEntete, List<API_LT_DOCLIGNE> lignes)
        {
            SageOM sageOM = new SageOM();
            var oCial = sageOM.CIAL();
            var oCpta = sageOM.CPTA();
            var rs = new AR_Result();
            try
            {
                if (oCial.FactoryDocumentVente.ExistPiece(GetDocumentType(docEntete.DO_Type), docEntete.DO_Piece))
                {
                    IBODocumentVente3 mDoc = (IBODocumentVente3)oCial.FactoryDocumentVente.ReadPiece(GetDocumentType(docEntete.DO_Type), docEntete.DO_Piece);

                    foreach (API_LT_DOCLIGNE art in lignes)
                    {
                        try
                        {
                            rs.ActiveRef = art.AR_Ref;
                            var mLig = (IBODocumentVenteLigne3)mDoc.FactoryDocumentLigne.Create();

                            if (String.IsNullOrEmpty(art.AR_Ref))
                            {
                                mLig.DL_Design = art.DL_Design;
                            }
                            else
                            {
                                mLig.SetDefaultArticle((IBOArticle3)oCial.FactoryArticle.ReadReference(art.AR_Ref), 1);
                                mLig.SetDefaultRemise();
                                mLig.DL_PrixRU = mLig.DL_CMUP;
                                mLig.DL_Qte = (double)(art.DL_Qte ?? 1);
                            }

                            mLig.WriteDefault();
                            rs.Oks.Add(art.AR_Ref);
                        }
                        catch (Exception ex)
                        {
                            // Log error for this specific article but continue processing others
                            rs.Errors.Add(new ArtError
                            {
                                AR_Ref = art.AR_Ref,
                                ErrorMessage = ex.Message
                            });
                            // Optionally log to a more detailed error collection if needed
                        }
                    }

                    mDoc.RecalculTotaux(true);
                    mDoc.WriteDefault();
                }

                // Return success if all processed, or mixed results if some failed
                if (rs.Errors.Count == 0)
                {
                    return "Ok";
                }
                else
                {
                    // Return serialized result that includes both successes and errors
                    return JsonSerializer.Serialize(rs);
                }
            }
            catch (Exception ex)
            {
                // Global exception that stops the entire process
                rs.Erreur = ex.Message;
                return JsonSerializer.Serialize(rs);
            }
            finally
            {
                if (oCial != null) CloseBase(oCial);
                //if (oCpta != null) CloseBaseCpta(oCpta);
            }
        }

        // You'll need to define these classes if they don't exist:
        public class AR_Result
        {
            public string ActiveRef { get; set; }
            public List<string> Oks { get; set; } = new List<string>();
            public List<ArtError> Errors { get; set; } = new List<ArtError>();
            public string Erreur { get; set; }
        }

        public class ArtError
        {
            public string AR_Ref { get; set; }
            public string ErrorMessage { get; set; }
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