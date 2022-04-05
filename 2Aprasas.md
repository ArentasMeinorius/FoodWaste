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

**The main tasks of our project are:** \
   1. Use UML 4+1 framework for organizing the architecture document. \
   2. Implement the planned changes in the system. \
   3. Introduce a CI/ID process. \
\clearpage
\tableofcontents
\clearpage

# Context
For a system to be successful, it must be developed with the intention of solving a real-world problem, which, in our case, is reducing food waste in restaurants and shops. The software is useless if it does not solve required problem. In this part we analyse our problem and how it is intended to be solved. 

## Goal of the system
Reduce food waste by distributing it.

### The problem
Not all food products are sold before spoiling, sometimes restaurants do not use all the food they have bought.

### Solution
Prepare a plaftorm that would stand as a middle man helping people sell excess food while  allowing others to buy it cheaper.

### Main User Goals
Monetary gain:
 + Providers: utilising over-booked food
 + Users: acquire food cheaper
Reducing food waste

## Planned changes
To further develop and increase the functionality of the existing system we were given several tasks of implementing changes and features. The improvement consists of adding food status to facilitate consumers lifes and by adding restaurant/grocery store edit option.

### Change list
 + Product status implementation, when it is bought etc.\
 + Restaurant accounts should be able edit their data.\
 + Add tech support.

\clearpage
### Impact of changes
The newly added functionality of ordering products will help automate the process of food receiving. Users will no longer need to personally contact food owners or have cash prepared as transactions will go online to payment provider. Additionally, newly added tech support will be able to help customers with any concerning questions that might arise while using the system. That will further improve the system usability for all types of users.
Finally, the ability for restaurants to easily edit their data should provide better up to date information which is always important to avoid misunderstandings with the users.

## Current system analysis
The current system only worked as a showcasing tool for the products, therefore it was hardly usable and inconvenient. The need to manualy contact food owners and negotiate made the process slow

### System environment
The main web-application server will be run on a linux machine using IIS Express. The database will be on the same server. Users will be able to connect through any type of modern browser. Support for older browsers (e.g., Internet Explorer) is not included. 

### Tools and Technologies
The main development tools and technologies, which are .NET Core Software framework, ASP.NET server-side application and razor framework. The database management system chosen by the original developers was Docker and and PostgreSQL. 
Overall, the tools and technologies are well chosen for the system in development. 

\clearpage
### Existing problems
User interface is too simplistic and lacks visual representation. There is too much text and zero appeal

   ![Food waste Product View](Assets/ProductView.png "Product view")

## Development environment
Version control systems, play a major role in any modern software development project. This is especially important for us, since out team will mostly work remotely. Our version control system is Git. The source code is hosted on GitHub, because all of the members are familiar with this repository management tool. Following good coding practices, every new feature implementation will be created in a separate branch and reviewed by at least one team member.

\clearpage
# Development view
The development view illustrates a system from programmer’s perspective and is concerned with software management. This view contains:\
1. Component diagram

## Component diagram
Component diagram provides high level architecture overview of different components used for operating Food Waste

![Food waste Component diagram](Assets/ComponentDiagram.jpg "Component diagram")

\clearpage
# Logical view
Logical view is concerned with the functionality that the system provides to end-users. This will be achieved via these diagrams: \
1. Class diagrams \
2. State machine diagrams 

Each of these diagrams has a separate section in which diagrams itself and descriptions are provided.\

## Class diagrams
The class diagram shown belown illustrates our application after the changes. We have included a new functionality of ordering the products and designed tech support. Also we kept in mind the necessity to manage restaurants and added some additional operations. This diagram allows us to implement the changes more easily with its structured view.

![Food waste Class diagram](Assets/ClassDiagram3.jpg "Class diagram")

\clearpage
## State machine diagrams
The state diagram shown below illustrates how our ordering systems works in happy day scenario. We can see that user adds or removes products to his order as he wishes and then proceeds to the payment. Once the payment is started, order status gets updated throughout the process and transaction is being verified. When the payment gets verified and customer gets his bill order is considered to be finished.

![Food waste State diagram](Assets/StateElements4.jpg "State diagram")

\clearpage
# Process view
Process view illustrates and explains the system processes. The focus is on their communication and synchronization. This view contains: \
1. Communication diagrams\
2. Activity diagrams

## Communication diagrams
![User purchases communication diagram](Assets/UserPurchasesCommunicationDiagram.png "Food waste communication diagram")
This communication diagram shows how components are supposed to communicate with one another when a user wants to make a purchase. As you can see, the user can browse available products without logging in, but when he wants to make a purchase he has to provide credentials. After that the user makes a purchase, the database is updated accordingly and the accounting is notified of the transaction.
\
\
\

![Provider changes communication diagram](Assets/ProviderCommunicationDiagram.png "Food waste communication diagram")
The provider needs the possibility to update information about himself, so this communication diagram shows how the process should go. Like a regular user, the provider can freely browse the products, but if he wants to make a change he must log in. After logging in, he is provided data about his account and his supplied products. He can make changes to this information, which is later saved into the data base. All changes are monitored.


## Activity diagrams
Tech support until this point was virtually non-existent, so there is not much to compare it to. The diagram describes how a user (in this case a logged in provider) should deal with a system error that does not allow him to properly continue his work. As is shown in the diagram, after an error the system logs the circumstances under which the error occurred and asks the provider if he wants to issue a ticket. If he does so, the tech support personnel review the problem, communicate with the provider and fix a problem that the person is having. After all this, the provider can return to his work.

![Food waste activity diagram](Assets/ActivityDiagram.png "Activity diagram")
\clearpage

# Physical view
In this part we analysed the topology of software components on the physical layer as well as physical connections between these components. 
This view contains:

1. Deployment diagram

## Deployment diagram
![Food waste deployment diagram](Assets/DeploymentDiagram.png "Deployment diagram")

# Use Case View
## Main use cases – diagram and description
## Admin use cases – diagram and description
## User use cases – diagram and description
# Traceability
