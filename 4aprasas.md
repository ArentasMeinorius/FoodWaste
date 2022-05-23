---
papersize:
- a4
fontsize:
- 12pt
geometry:
- margin=1in
fontfamily:
- charter
header-includes:
- \setlength\parindent{24pt}
---



\setcounter{page}{1}
\pagenumbering{arabic}
\begin{titlepage}
   \begin{center}
       \vspace*{1cm}

       \textbf{Food Waste}

       \vspace{1.5cm}

       \textbf{Arentas Meinorius,\\Jaunius Tamulevičius,\\Martinas Mačernius,\\Pijus Petkevičius}

       \vfill

       \vspace{0.8cm}

       Matematikos ir informatikos fakultetas\\
       Vilniaus universitetas\\
       Lietuva\\
       \today

   \end{center}
\end{titlepage}



# Summary {.unlisted .unnumbered}
&nbsp;&nbsp;&nbsp;&nbsp;The primary objective of the second laboratory assignment is to design the system and required changes. While in the first laboratory work we analysed business and all its processes, this time the attention on existing system and the changes. \

**The main tasks of this iteration:** \
   1. Allergens feature \
   2. E2E test \
   3. Document the E2E test \
   4. Viewpoints \
   5. Perspectives \
   6. Traceablity tables \
\clearpage
\tableofcontents
\clearpage

\clearpage
# Viewpoints

## Context viewpoint

### Context system scope and responsibilities:

The whole system:
FoodWaste systems tries to eliminate hunger and binned food. Providers such as shops and restaurants inform users about foods that would be thrown away in the near future by giving them access to before mentioned food. Users that are interested in zero waste lifestyle as well as those in a financial troubles are welcome to use FoodWaste.

The system update:
Allergen functionality aims to protect users from the ingredients that are harmful to them.

- Allergens are properties of food products that are likely to cause unwanted reactions with allergic individuals.

    - All food offerings may contain a list of allergens that have to be set by the restaurant
    - All users may provide a list of allergens that are dangerous to them \

- System will use the user and product allergen information to:

    - Alert if user is trying reserve product that may be dangerous to the individual
    - Provide easy filtering of allergic products (in the future)

### Context external entities and services and data used:

The whole system:
FoodWaste uses external providers for: authentication, monitoring, data storing, payments, accounting and live support agents.

The system update:
Annex 2 naming system for allergens will be used. No other external entities are concerned with this change.

### Context impact of the system on its environment:

The whole system:
FoodWaste uses CI to automate deployment process so no system should be impacted by different updates. Restaurants and shops that use FoodWaste API will have to make updates to their systems when there will be any breaking changes or new feature releases.

The system update:
Not applicable.


### Context consistency and coherence

The whole system:
FoodWaste uses single point of truth: the database. Data is inserted via transactions to ensure validity of entries. Validation of information is executed in the FE as well as BE. Currently the system is not complex enough for coherence problems to arise.

The system update:
Not applicable.


## Functional viewpoint

### Functional capabilities:

The whole system:
FoodWaste provides platform for providers and users to list, reserve and retrieve foods that would be thrown away in the near future. FoodWaste does not provide transportation or courier services.

The system update:
Allergen functionality provides information on allergens and alerts. It does not protect users that ignore warnings.

- Each restaurant will have an option to add allergens to products:

  1. While creating offers
  2. Anytime while offer is not reserved

- Each restaurant will have an option to remove allergens from offers anytime while products are not reserved
- Each user will be able to add allergens to their profile anytime

### Functional external interfaces:

The whole system:
FoodWaste communicates with providers systems to automate the process of listing foods. Providers may use GUI if they choose to.  Other systems are used to ensure functional work of FoodWaste (described in context)

The system update:
Allergen functionality uses only Annex2 naming system.

### Functional functional design philosophy:

The whole system:
FoodWaste aims to reduce food waste while helping individuals. This is a non profit system looking for better ways to utilize planet's resources. 

The system update:
Allergen functionality aims to provide all the tools for the individuals to choose foods that do not harm them.

## Information viewpoint

- Allergen entity contains name, [Annex 2 name](https://ec.europa.eu/food/system/files/2018-07/codex_ccfl_cl-2018-24_ann-02.pdf), severity: [May contain, Contains]
- Product entity contains list of allergens
- User entity contains list of allergens that are dangerous to them

## Concurrency viewpoint

Not applicable. Allergens are properties, not processes that could happen concurrently.

## Development viewpoint

Allergen in FoodWaste system is not a major architectural component. Allergen lists are attached to major components such as product and user, however, they do not interfere with current usage of these entities and their functionality (except for the alert if trying to reserve allergic foods). In the future, it should be easy to implement allergen filtering on main page (for better user experience).

\clearpage

## Deployment viewpoint

Allergens are properties, not major components that change the program environment or its execution. Deployment will not differ from other feature deployments (A/B testing in production environment for part of the users and if there are no incidents - major release).

## Operational viewpoint

- Users will provide their allergens when registering or anytime when using FoodWaste
- Users will be alerted if trying to reserve products they are allergic to
- Restaurants will have to provide allergen list for their current products and each time when creating new offers

\clearpage
