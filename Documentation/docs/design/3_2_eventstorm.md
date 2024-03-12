### Eventstorm

To kickstart the design process for Kwetter, I adopted the Eventstorming technique.
Eventstorming is a collaborative workshop approach that helps visualize the flow of events and interactions within a
system.
It allowed me to gain a comprehensive understanding of the business areas and identify the key events and processes
involved in Kwetter's functionality.

During the Eventstorming session, I brought together stakeholders (me), domain experts (still me), and the development
team (also me) to collectively map out the various activities and events that occur within the social media platform.
I used an online whiteboard tool to capture these events and their relationships.

![](images/eventstorm_kwetter.png)

The Eventstorming session provided us with the following benefits:

1. Overview of Business Areas: By visualizing the events and their sequences, we gained a clear overview of the different
business areas within Kwetter.
This included user registration, Kweet posting, interactions (replies, likes, rekweets), user profiles, trending topics,
and moderation functionalities.

2. Identification of Key Events: Eventstorming helped us identify the essential events that form the core of Kwetter's
functionality.
These events served as the building blocks for defining the system's features and interactions.
Examples of key events in Kwetter included "User registers," "Kweet posted," "User follows another user," and "Kweet
replied to."

3. Understanding User Roles and Interactions: Through Eventstorming, we visualized how different user roles interacted with
the system and with each other.
This allowed us to identify the necessary features and functionalities for each role, such as user profile management
for registered users and moderation capabilities for administrators.

4. Determining Microservices and Initial Architecture: Based on the identified business areas and events, we determined the
microservices that would form the initial architecture of Kwetter.
Each microservice would be responsible for handling specific business functions or processes.
For example, we recognized the need for microservices such as User Management, Kweet Service, Trending Service, and
Moderation Service.

The insights and outcomes from the Eventstorming session served as a foundation for subsequent design activities, such
as system architecture, API definitions, and database schema design.
They helped me prioritize the development efforts and determine the initial set of services to be implemented, ensuring
that the architecture aligned with the identified business areas and events.

Throughout the design and development process, we referred back to the Eventstorming artifacts to validate and refine
our understanding of the system's behavior and to guide the implementation of features.
This approach enabled me to build a well-informed and user-centric social media platform with Kwetter.

