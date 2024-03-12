## Analysis

Let's take a look at some of the technical challenges, problems and questions that arise based on both the functional and nonfunctional requirements of Kwetter:

1. It should be able to support a large userbase from all over the world
2. The functionality is based on high engagement, and encourages active use throughout the day
3. The project must be implemented using microservices 
4. The combination of microservices, high engagement, and the core functionality of posting and replying means that different services have to run in sync
5. Large amounts of data have to be stored, served to users and analyzed for trends
6. We want to ensure that user posts are accurate and are retained for the life of the application
7. 
These issues cover a wide range, but can be encapsulated under the following problem statement:

**Design a web application that allows people from all over the world to share, discuss and find their thoughts with the knowledge that they can do so at any time and without those discussions being lost.**

---

## Research Questions

The focus of this project is to learn about various technologies and techniques used in enterprise software, ranging from microservices to design patterns, code quality tools and infrastructure.
Thus, the focus of the research questions will also be aimed in that direction.
This leads to the fact that “Choose Fitting Technology” and “Realize as an Expert” are very suitable research patterns, since both focus on using and/or analyzing existing solutions within the field, and applying the gained knowledge to the problem at hand.
Due to this, I will for now base my research on these two patterns.

Besides the following research I will be conducting various smaller research projects to establish a base knowledge about relevant subjects. These will be detailed only if their scope grows to the point where it’s worth properly documenting them.

### What solution(s) can ensure that Kwetter can support a growing user base across the world?

*\[Choose Fitting Technology]*

1. *\[Library]* What technologies are available for:
    - Scaling application capacity
    - Scaling application infrastructure
2. *\[Field]* What requirements does Kwetter have that a solution has to account for?
3. *\[Workshop]* Create prototypes for the various possible technologies within our domain.
4. *\[Lab]* Test the suitability of these prototypes and combinations of these prototypes.

The results of this research can be found [here](scalability/scalability.md).

### What is needed to ensure the integrity and retention of Kweets?

*\[Choose Fitting Technology]*

1. *\[Library]* What technologies are used for large volume data retention?
2. *\[Field]* What requirements does Kwetter have that a solution has to account for?
3. *\[Workshop]* Create prototypes for the various possible technologies within our domain.
4. *\[Lab]* Test the suitability of these prototypes.

---

Besides Kwetter, I want to highlight my research I did for the Rekeningrijden project [here](https://structurizr.com/share/83409/documentation/Research%202:%20Routing).

### How do we generate and process routes to simulate traffic for Rekeningrijden?

For this research the process was similar to *\[Choose Fitting Technology]* or *\[Realise as an Expert]* but neither of those fully fits the project.

1. *\[Library]* What datasets are available for generating routes?
2. *\[Field]* What should the trackers send, and how?
3. *\[Field]* What format should the final data have?
4. *\[Workshop]* How do we go from dataset to route?
5. *\[Workshop]* How can we transform the tracker data back into a route?
6. *\[Workshop]* How can we ensure that transformation is scalable?
