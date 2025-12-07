# 🔍 Filtrage et Recherche de Véhicules

## Fonctionnalités Ajoutées

### 1. **Barre de Recherche Principale**
- Recherche par **marque, modèle, immatriculation et couleur**
- Recherche en temps réel (case-insensitive)
- Interface intuitive avec icône de loupe

### 2. **Filtres Disponibles**
- **Marque** : Liste dynamique de toutes les marques disponibles
- **Disponibilité** : Filtrer véhicules disponibles / non disponibles
- **Prix** : Filtrage par plage de prix (min et max)
  - Incréments de 10 (step="10")
  - Min: 0

### 3. **Actions de Filtrage**
- **Bouton Filtrer** : Applique tous les critères de recherche
- **Bouton Réinitialiser** : Efface tous les filtres (redirection vers /Vehicules)
- **Compteur de résultats** : Affiche le nombre de véhicules trouvés
- **Indicateur de recherche active** : Bouton pour effacer les filtres appliqués

### 4. **Interface Utilisateur**
- **Carte de recherche** : Conteneur blanc avec ombre (shadow-sm)
- **Filtres responsifs** : Adaptation automatique sur mobile/tablette/desktop
- **Messages contextuels** :
  - ✅ Affichage du nombre de résultats
  - ⚠️ Message d'alerte si aucun résultat
  - 📝 Suggestions pour réessayer avec d'autres critères

## Fichiers Modifiés

### 1. **Pages/Vehicules/Index.cshtml.cs**
```csharp
// Propriétés ajoutées pour le filtrage
[BindProperty(SupportsGet = true)]
public string? SearchQuery { get; set; }
public string? FilterMarque { get; set; }
public decimal? FilterPrixMin { get; set; }
public decimal? FilterPrixMax { get; set; }
public bool? FilterDisponible { get; set; }

// Propriétés pour l'affichage
public List<VehiculeDTO>? VehiculesFiltered { get; set; }
public List<string>? Marques { get; set; }

// Méthode de filtrage
private void ApplyFilters()
{
    // Filtrage par recherche (marque, modèle, immatriculation, couleur)
    // Filtrage par marque
    // Filtrage par prix min/max
    // Filtrage par disponibilité
}
```

### 2. **Pages/Vehicules/Index.cshtml**
```html
<!-- Nouvelle section de recherche et filtrage -->
<div class="card shadow-sm mb-4 border-0">
    <form method="get">
        <!-- Barre de recherche -->
        <!-- Sélecteur de marque -->
        <!-- Filtre de disponibilité -->
        <!-- Boutons d'action -->
        <!-- Filtres avancés (prix) -->
    </form>
</div>

<!-- Affichage dynamique des résultats -->
<!-- Alerte avec compteur de résultats -->
<!-- Grille de véhicules filtrés -->
```

## Flux de Traitement

```
Utilisateur entre des critères
           ↓
[GET request avec les paramètres]
           ↓
OnGetAsync() charge les véhicules
           ↓
ApplyFilters() applique chaque filtre
           ↓
VehiculesFiltered = résultats filtrés
           ↓
La vue affiche VehiculesFiltered
```

## Exemples d'Utilisation

### Recherche par texte
```
SearchQuery = "bmw" → trouve tous les BMW
SearchQuery = "ABC123" → trouve l'immatriculation ABC123
SearchQuery = "bleu" → trouve toutes les voitures bleues
```

### Filtrage par marque
```
FilterMarque = "Toyota" → affiche uniquement les Toyota
```

### Filtrage par disponibilité
```
FilterDisponible = true → affiche les véhicules disponibles
FilterDisponible = false → affiche les véhicules loués
```

### Filtrage par prix
```
FilterPrixMin = 50, FilterPrixMax = 150 
→ affiche les véhicules entre 50 et 150 par jour
```

### Combinaison de filtres
```
SearchQuery = "audi" + FilterPrixMax = 100
→ affiche les Audi à moins de 100 par jour
```

## Points Clés

✅ **Recherche Case-Insensitive** : "BMW", "bmw", "Bmw" trouvent la même voiture
✅ **Filtres Combinables** : Tous les filtres peuvent être utilisés ensemble
✅ **URL Friendly** : Les paramètres apparaissent dans l'URL (ex: ?SearchQuery=bmw&FilterMarque=BMW)
✅ **Responsive Design** : Interface adaptée à tous les appareils
✅ **Retour d'information** : Compteur et message de résultats visibles

## État d'Utilisation

- ✅ Build réussi sans erreurs
- ✅ 12 Warnings (nullability) - non bloquants
- ✅ Serveur accessible sur http://localhost:5180/Vehicules
- ✅ Prêt pour les tests en production
