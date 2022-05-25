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

#Perspectives

##Accessibility perspective

##Availability and Resilience perspective

###On Concurrency view
The whole system:
The system should have redundant hardware in case of the used one failing. Not being able to sell our food products would mean that a lot of them would go to waste and we, as well as our partners, would lose out on profits.  Because we do not have a local database we should look into having multiple providers to host our data in case one of them would fail to provide their services. Our system should also have an automated failover in case the hardware fails. Best case scenario would be that the emergency system could take over the work of the failed system.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Deployment view
The whole system:
The redundant hardware should always be ready for deployment and up to date. During the deployment the system should be autonomous enough to know which database is ready to use and in what state were the users left when the system failed.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Context view
The whole system:
Because we do not use a local database, we could have two remote data storage providers handle our data. This should be taken into consideration as one database will be the active one, meaning if it fails the other database will not be up to date. We should prepare for this by either having extremely frequent database updates for the redundant database or by having a reasonable system that could estimate how many products could be left in our inventory based on how fast they usually sell out and what was the last time the database was updated.

The system update:
As allergens are important to the health and wellbeing of our users, therefore is we would lose any information that they have given us we should have notifications in place that would warn the user that they should double check the allergen tab in the product page to make sure they are buying a product that is safe for their consumption.

###On Functional view
The whole system:
Unfortunately, due to the nature of our system, having a network connection and a working system is essential if users want to use our system as it was intended. If the system would totally collapse or the user would lose internet connection for a long period of time the user would be left without any way to interact with the system. If we were to allow some accessibility to the user when they are offline to, for example, look through their profile or order history, it would be a violation of user data security and should not be allowed.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Information view
The whole system:
As was written before, we do not use a local database. We should use two database storage providers in case the provider goes offline for whatever reason. The system should be constantly communicating with the database so that it could tell when the database is inaccessible as soon as possible. In theory, transitioning to the other database should not take too much time. The backup database should be updated every night when there are minimal number of users using the system, as the process should be lengthy.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Operational view
The whole system:
In case of a system hardware failure or database storage supplier going down, the system should be ready to make a swift diagnostic of what services are unavailable and what course of action should be taken. The entire process must be tested beforehand.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

##Development Resource perspective

##Evolution perspective

##Internalization perspective

##Location perspective

###On Concurrency view
The whole system:
Because our database is accessed remotely, the latency can be slightly higher than normal. Knowing that the same record from it won’t be accessible to multiple users combined with the latency can result in long query times. Therefore, the system should be able to determine on its own how many products it can sell to the user before noting the change in stock in the database when the needed record is not occupied. This means that the system should always have a limit on the quantity of a single product that the user can purchase at a given timeframe.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Deployment view
The whole system:
Because the only separate part of our system is the database, the disparity is not a big issue. We should take into consideration to have our queries and their results as short as possible, as the database is going to be used constantly.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Context view
The whole system:
The database storage providers should be based in Europe, as it would ensure we have minimal latency and reliable availability

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Information view
The whole system:
The main database should be used to update the backup database. The update would take place every night when the system experiences minimal user activity. If a provider were to go offline the system should be able to swiftly diagnose the disconnect and start using the backup database. Of course, if the disconnect takes places in the evening, the data will be inaccurate. The system should have access to some statistical measurements of how fast certain food products are bought on what times of day and roughly assess how much food products should be left in stock and could be sold while keeping tabs of current transactions to update the main database when it comes back online. It is certain that separate providers will be able to synchronize the database information on their own, so during the night the system should compare the stock of main database to the stock of the backup one and issue update commands on its own.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Operational view
The whole system:
Because we are using a provider that specializes in database hosting, we believe that they can be trusted to deliver their best efforts to keep their services running. If any unforeseen event occurs, we trust that they will recover within a couple of hours.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

##Performance and Scalability perspective

##Regulaiton perspective
###On Context view
The whole system:
Our system should be able to generate monthly reports to an accounting company so that they could work on salary calculations, audit reports, etc.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Functional view
The whole system:
Because we are dealing with food that is near its expiration, we should take we partner with companies that can ensure, that their provided food products comply with “Lietuvos Respublikos maisto įstatymas”, document number VIII-1608.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

###On Information view
The whole system:
Because we operate in Europe, we have to be able to give a  request to our users about their collecting and managing information about the, give a report to our users about the data we have collected on them, give them the possibility to delete this data, to notify them within 72 hours of data leaks, ant not to hold onto any user information that we do not need to hold on to.

The system update:
We will have to ask for a permission to collect and manage data about user allergies. Allergens will be deleted on request of the user or on user profile termination.

###On Operational view
The whole system:
We need to be able to automatically give out SLAs to our users to be able to provide our services. This includes basic services, cookie usage, etc.

The system update:
Because the update added an extra functionality, the same points apply to it as they do for the whole system.

##Security perspective

##Usability perspective