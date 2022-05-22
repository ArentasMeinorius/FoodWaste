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

- Allergens are properties of food products that are likely to cause unwanted reactions with allergic individuals.

    - All food offerings may contain a list of allergens that have to be set by the restaurant
    - All users may provide a list of allergens that are dangerous to them \

- System will use the user and product allergen information to:

    - Alert if user is trying reserve product that may be dangerous to the individual
    - Provide easy filtering of allergic products (in the future)

## Functional viewpoint

- Each restaurant will have an option to add allergens to products:

  1. While creating offers
  2. Anytime while offer is not reserved

- Each restaurant will have an option to remove allergens from offers anytime while products are not reserved
- Each user will be able to add allergens to their profile anytime

## Information viewpoint

### Information Structure and Content

The whole system:
The main information that our system manipulates is user data, restaurant information and its food products. User class has all the information necessary to effectively communicate with them as well as information on their enjoyed products and restaurants. The restaurants have information about their location, contact information and mainly about their sold food products. The food product has basic information like its name, cost and description.

The system update:
The update added a new Allergen class to the system. It carries the name and description information of an allergen. It is used by the User and FoodProduct classes. For user it records what the user is allergic to, and for FoodProduct it records what kind of allergens it has.

### Information Purpose and Usage

The whole system:
Our system uses information in multiple ways. There is contact information – it is used to get into contact with users or restaurants outside of our system. This information includes email, phone number, address, etc.  Then there is reservation information – it has the information about users that have reserved some products. For example, there are reserved items, payment information, food products, etc. Then we have the payment information with cost, date, account numbers. And finally, we have information about user preference. It includes favoured restaurants, favoured food products, notifications, etc.

The system update:
The update added a new Allergen information to the system. The purpose of this information is to avoid any deadly allergic reaction for our users. It is used to match what our users know they are allergic to with products that can cause a reaction and to notify the user of the present danger to their health.

### Information Ownership

The whole system:
The system so far is only planned to have a single data store so no information conflicts should happen. Any choice by the user is only recorded in the database only when it is for sure final and can not have information conflicts due to, for example, infrequent synchronisation with the main database.

The system update:
No changes in the number of data stores, so the system works the same as was descried above.

### Enterprise-Owned Information

The whole system:
The enterprise-owned information would be the restaurant information and their sold products. It would be a serious inconvenience to, for example, sell a product to a user that the restaurant does not has anymore, so this information should be updated frequently.

The system update:
The change adds allergen information. It is extremely important to have updated and correct form of this information. Havin too many allergens than user has means we sell less food products and having to few allergens means that the user can have a dangerous allergic reaction to their health, at beast reducing their trust in our system and at worst costing us the life of our customer

### Identifiers and Mappings

The whole system:
From the systems perspective every single class that can be unique has a unique guid that allows it to be differentiated. Unique guid is given to users, food products and restaurants. From user perspective the unique guid is not shown, the system makes sure that user would only see unique products. The restaurant adds a new food product by just entering its name, not selecting it from existing ones. The possible problem of creating an entirely new food product by misspelling an existing one is not a big problem, as it should not cause any big mix ups, and considering, that there might be a lot of changing products in our system it would not be wise to have a long process of adding a new food product to a list of possible to update food products.

The system update:
The change adds allergen information. From system perspective the allergen has a unique guid. From user perspective the guid is not shown. A user can select only an already existing allergen in the system, as to prevent any misspelling of allergen, which could result in a user having no waring about a dangerous allergen that he has recorded in his profile simply because the allergen was misspelled.

### Volatility of Information Semantics

The whole system:
No attention was paid when thinking about possible future changes of the system, as no work is planned to be done to this system after this update.

The system update:
Same as above.

### Information Storage Models

The whole system:
The system uses a basic third-normal-form relational database, managed with PostgreSQL. It is a standard solution for a simple system like ours, providing good performance, flexibility and avoids data duplication if design correctly.

The system update:
We continue using third-normal-form relational database.

### Information Flow

The whole system:
The data is created when a new user profile is created or updated with new information, and when a restaurant adds a new food product. Information is destroyed when a user deletes his profile, updates his profile by removing information or when products expiration date is reached or when a product is bought. This data is used when user is browsing food products in our system. This is where collected data is shown, combined, compared. The change in data items happen when food product state is changed.

The system update:
Allergen information is created periodically when new products that have new allergens are added into a system. The allergens are not meant to be destroyed. Products and users can change to what allergens they have or are allergic to. This data can be accessed or modified by the user in the profile tab. It is made use of when browsing food products and being warned of dangerous ones.

### Information Consistency

The whole system:
Our system will use atomic operations to ensure, that any order or action will have its intended full effect rather than a half-done operation that will leave us or the user in a losing situation.

The system update:
Allergens don’t have any way to be inconsistent.

### Information Quality

The whole system:
The information given by the restaurants is to be trusted as high-quality information, as we expect to work with professionals. If they provide low quality information they are the ones that will suffer from it. For the users it depends mainly on how active they are. The main information that will age fast is their address, but if they use our system often they will notice, that any order is destined to go to their old address, so it is on them to update it.

The system update:
Only medically proven allergens will be added to our system. No user will have to enter their own allergen – they will already choose an allergen that is deemed to be dangerous.

### Timeliness, Latency, and Age

The whole system:
Our information is held in a single data store and is always accessed synchronously in real time, timeliness, latency, and age are not significant issues. The system keeps track when product is spoiled and should be removed from store, so there is no need to constantly update information. 

The system update:
Allergen information doesn’t age, so it does not cause any problems as well.

### Archiving and Information Retention

The whole system:
There is no useful information in our system that should be archived after it is no longer needed. It is illegal to keep personal information of a person for no reason and there is no need to know about spoiled food that we once had. Even if we would collect this information we couldn’t effectively use it as we don’t control the supply of it – we are supposed to take in any product that is close to expiration. Knowing that usually we usually sell out all specified products will not mean that we will be able to get more of it.

The system update:
Allergen information shouldn’t really be deleted or archived, so no changes from the text above.

## Concurrency viewpoint

Not applicable. Allergens are properties, not processes that could happen concurrently.

## Development viewpoint

Allergen in FoodWaste system is not a major architectural component. Allergen lists are attached to major components such as product and user, however, they do not interfere with current usage of these entities and their functionality (except for the alert if trying to reserve allergic foods). In the future, it should be easy to implement allergen filtering on main page (for better user experience).

\clearpage

## Deployment viewpoint

Allergens are properties, not major components that change the program environment or its execution. Deployment will not differ from other feature deployments (A/B testing in production environment for part of the users and if there are no incidents - major release).

## Operational viewpoint

### Instalation and Upgrade

The whole system:
Our system is planned to run on our customer machine, so the installation and upgrade procedures should be relatively easy, as we only need to worry about limited hardware specification. The installation and upgrades are not fully automated, no request was given for such a convenience. Our system will be installed as a fresh product, no other similar system operated fully before this one, there are no software that needs to be relieved. But any future changes will have to be upgrades to the running system rather than completely new installations, as the system will have to work with old users as if nothing changed.

The system update:
The implemented changes do not radically change the system and will allow for an easy upgrade. The hardware requirements are the same, only some additional information is logged in the database, so no radical changes to existing record are needed, only an addition of an empty field to all existing food products and to the allergens of all users.

### Functional Migration

The whole system:
As the old version of the system was minimally used, our system can be migrated using the big bang approach, as no extensive use is planned in the future. Due to having a limited operational window, there is nothing to lose in case of an unsuccessful migration.

The system update:
The system is used in a limited way, so there is no risk of damage from unsuccessful migration. We will use the big bang approach.

### Data Migration

The whole system:
The system did not operate with any actual users, neither before or after our work on it, so any existing data on it is fabricated and purely for testing purposes and has no need to be migrated. Even if the data is required to be migrated it can easily be done manually due to its small size.

The system update:
The system data had a slight change due to addition of allergens, but just like before, no actual user has used the system, so any migration currently has no huge meaning and can be done manually or not even done at all.

### Operational Monitoring and Control

The whole system:
A system with monetary transactions would benefit from monitoring and control operations. There were monitoring functionalities in the system given to us but there were no requests by the customer to develop control operations or expand on the monitoring in the system, so currently no control operations exists in this system.

The system update:
The update was the addition of allergens to the system user profile and food products. No in-depth monitoring or control operations over this update is required nor was requested, so none were developed.

### Alerting

The whole system:
No alert system is currently present in the system and no request to develop one was given. Any unforeseen events occurring are meant to be reported to the developer team.

The system update:
The update was the addition of allergens to the system user profile and food products. Although this means that there are more moving parts that can fail, no alert system has been requested ant there is no base that we could easily build upon.

### Configuration Management

The whole system:
Because our database does not run on the local machine, it has a separate configuration from the system. Other than that there are no foreseen requirements for handling different configurations – currently only one is used.

The system update:
No new configurations have been made with the update.

### Performance Monitoring

The whole system:
No additional performance monitoring is performed outside of viewing how the system runs on the used machine. The Visual Studio IDE can give some statistics of the systems performance during run time, no other way of collecting performance information was requested by the customer.

The system update:
The update focused on allergens, no work on system monitoring was done, nor did it had a great effect on the systems performance that would warrant such work to be done.

### Support

The whole system:
We discussed how technical support could be implemented in the second laboratory work. We did not choose to implement that change into the system, but if any need arises in the future the basic idea of it is already though through. 

The system update:
We wanted to be as clear as possible with the allergens in the user interface itself. We believe we have made an easily comprehensive system and that no extra customer support is needed.

### Backup and Restore

The whole system:
Our system uses only a single database, so no need to prepare for a complicated recovery over multiple data stores. Making a backup would usually take place every night, when data changes the least, but with current usage of the system (which is minimal) the backup process could happen every weekend. The backup copy would be saved in a separate drive, not in the active one in the system.

The system update:
No changes were made to the process of backing up data.

### Operation in Third-Party Environments

The whole system:
Our system uses a database hosted on a remote machine. Although for a small-scale product, like ours currently is, it is not the most convenient solution, our customer was rather considerate of its long-term advantage and gave the system to us in such a state. We had some difficulties because of this decision as we struggled to even run the project properly as we received little detailed support from the client about the connection process. But we had received no request to change the system to use a local database, so it has stayed the way we received it.

The system update:
The update has not added any new third party environments nor removed any old ones.

\clearpage
