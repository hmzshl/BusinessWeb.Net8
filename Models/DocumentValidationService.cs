using BusinessWeb.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;

public class DocumentValidationService
{
	public ResultatsValidation ValiderDocuments(List<API_V_DOCENTETE> documents)
	{
		var resultats = new ResultatsValidation();

		if (documents == null || !documents.Any())
		{
			resultats.Erreurs.Add("Aucun document fourni pour validation");
			return resultats;
		}

		// Grouper par préfixe de document (caractères non numériques au début)
		var documentsGroupes = documents
			.Where(d => !string.IsNullOrEmpty(d.DO_Piece) && d.DO_Date.HasValue)
			.GroupBy(d => ExtrairePrefixe(d.DO_Piece))
			.ToList();

		foreach (var groupe in documentsGroupes)
		{
			var serie = groupe.Key;
			var documentsValides = groupe
				.Where(d => EssayerExtraireNumero(d.DO_Piece, out _))
				.OrderBy(d => ExtraireNumero(d.DO_Piece))
				.ToList();

			if (!documentsValides.Any()) continue;

			// Vérifier si la séquence commence à 1
			var premierNumero = ExtraireNumero(documentsValides.First().DO_Piece);
			if (premierNumero != 1)
			{
				resultats.AjouterErreurDebutSerie(serie, premierNumero);
			}

			int? numeroPrecedent = null;
			DateTime? datePrecedente = null;

			foreach (var doc in documentsValides)
			{
				int numeroActuel = ExtraireNumero(doc.DO_Piece);
				DateTime dateActuelle = doc.DO_Date.Value;

				// Vérifier les trous dans la numérotation
				if (numeroPrecedent.HasValue && numeroActuel != numeroPrecedent + 1)
				{
					resultats.AjouterErreurIntervalle(serie, numeroPrecedent.Value, numeroActuel);
				}

				// Vérifier l'ordre chronologique
				if (datePrecedente.HasValue && dateActuelle < datePrecedente)
				{
					resultats.AjouterErreurChronologique(serie, doc.DO_Piece, dateActuelle,
													   $"{serie}{numeroPrecedent}", datePrecedente.Value);
				}

				numeroPrecedent = numeroActuel;
				datePrecedente = dateActuelle;
			}
		}

		return resultats;
	}

	// Méthodes utilitaires
	private static string ExtrairePrefixe(string numeroDocument)
	{
		return new string(numeroDocument.TakeWhile(c => !char.IsDigit(c)).ToArray());
	}

	private static int ExtraireNumero(string numeroDocument)
	{
		string partieNumerique = new string(numeroDocument.SkipWhile(c => !char.IsDigit(c)).ToArray());
		return int.Parse(partieNumerique);
	}

	private static bool EssayerExtraireNumero(string numeroDocument, out int numero)
	{
		numero = 0;
		var caracteresNumeriques = numeroDocument.SkipWhile(c => !char.IsDigit(c)).ToArray();
		if (caracteresNumeriques.Length == 0) return false;
		return int.TryParse(new string(caracteresNumeriques), out numero);
	}
}

public class ResultatsValidation
{
	public List<string> Erreurs { get; } = new List<string>();
	public List<ErreurIntervalle> ErreursIntervalles { get; } = new List<ErreurIntervalle>();
	public List<ErreurChronologique> ErreursChronologiques { get; } = new List<ErreurChronologique>();
	public List<ErreurDebutSerie> ErreursDebutSeries { get; } = new List<ErreurDebutSerie>();

	public bool ADesErreurs => Erreurs.Any() || ErreursIntervalles.Any() ||
							  ErreursChronologiques.Any() || ErreursDebutSeries.Any();

	public void AjouterErreurIntervalle(string serie, int numeroPrecedent, int numeroActuel)
	{
		ErreursIntervalles.Add(new ErreurIntervalle
		{
			Serie = serie,
			ManquantsEntre = (numeroPrecedent + 1, numeroActuel - 1),
			Message = $"[{serie}] Intervalle manquant : Documents absents entre {numeroPrecedent + 1} et {numeroActuel - 1}"
		});
	}

	public void AjouterErreurChronologique(string serie, string documentActuel, DateTime dateActuelle,
										  string documentPrecedent, DateTime datePrecedente)
	{
		ErreursChronologiques.Add(new ErreurChronologique
		{
			Serie = serie,
			DocumentActuel = documentActuel,
			DateActuelle = dateActuelle,
			DocumentPrecedent = documentPrecedent,
			DatePrecedente = datePrecedente,
			Message = $"[{serie}] Problème chronologique : {documentActuel} ({dateActuelle:dd/MM/yyyy}) est avant {documentPrecedent} ({datePrecedente:dd/MM/yyyy})"
		});
	}

	public void AjouterErreurDebutSerie(string serie, int premierNumeroTrouve)
	{
		ErreursDebutSeries.Add(new ErreurDebutSerie
		{
			Serie = serie,
			PremierNumeroTrouve = premierNumeroTrouve,
			Message = $"[{serie}] La série ne commence pas à 1. Premier numéro trouvé : {premierNumeroTrouve}"
		});
	}
}

public class ErreurIntervalle
{
	public string Serie { get; set; }
	public (int Debut, int Fin) ManquantsEntre { get; set; }
	public string Message { get; set; }
}

public class ErreurChronologique
{
	public string Serie { get; set; }
	public string DocumentActuel { get; set; }
	public DateTime DateActuelle { get; set; }
	public string DocumentPrecedent { get; set; }
	public DateTime DatePrecedente { get; set; }
	public string Message { get; set; }
}

public class ErreurDebutSerie
{
	public string Serie { get; set; }
	public int PremierNumeroTrouve { get; set; }
	public string Message { get; set; }
}