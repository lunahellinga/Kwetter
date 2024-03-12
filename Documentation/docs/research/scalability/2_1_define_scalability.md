## What is scalability?

To start off with, we need to define scalability.
What makes a software system scalable - wether it be microservices or a monolith - is the ability to allocate resources
to different parts of the system as needed [A Detailed Guide to How to Scale Microservices | OpsLevel. (n.d.).].
In a monolith application this is difficult, since we don't have accurate control over which part of the software gets
what part of the resources.
The best we can do is throw more resources at the program and hope that it works.

Microservices, due to being built with separated responsibilities, allow us to do so accurately.
When one function of our system is seeing high usage, we can allocate resources to the service(s) that are responsible
for that functionality without wasting resources on other parts of the system that aren't heavily used.

### The Example

As a running example for different scaling techniques, we will be using an application that allows us to search for
vehicles various parameters such as type and model, and provides us with various data including example photos of said
vehicle.

### Scaling Methods

There's various ways of scaling an application.
Let's take a look at the primary vectors available here.

#### Just allocate more RAM

One way of scaling your system is by simply scaling that
instance [A Detailed Guide to How to Scale Microservices | OpsLevel. (n.d.).].
This means that when a certain part of the system is stressed, you increase the resources available to that system by
allocating more CPU or memory to the running containers.
This is the simplest way of scaling, since it allows for a service to deliver more performance without complicating its
design.

![](embed:rs_scale_mon)

While this is the easy solution, the problem is that its limited.
There's a limit to how much an application can scale, to what a platform can offer and to what the infrastructure can
support.
Sure, we can allocate 32 CPU cores to a single container, but that won't necessarily help with performance, and it will
often give diminishing returns as other resources or dependencies - such as databases - start limiting the gains.
It also introduces weaknesses into the system, as the scaled application failing will have a larger effect.

So, what solutions are more suitable for microservice based solutions?

#### X-axis scaling: horizontal duplication

[The Scale Cube. (n.d.).]

The first way to scale is X-axis scaling.
Instead of scaling the resources allocated to a single instance, you increase the number of instances and put them
behind a load balancer so each one handles one N<sup>th</sup> of the load.
This is a simple and often used solution, but comes with its own drawbacks.

![](embed:rs_scale_x)

"[...]it simply does not scale well with an increase in data,
either as instruction sets or reference data. The same holds true if the work varies by the sender or
receiver." - [Abbott, M. L., & Fisher, M. T. (2015).]

As the different functions of an application scale, more edge cases are introduced or more data is needed, a growing
part of the applications runtime is spent on overhead.
Data needs to be fetched and cached for each instance and various parts of the application need to be able to perform a
large number of different tasks, but are often just sitting there as the current task is done by another part of the
program.

#### Z-axis scaling: data partitioning

[The Scale Cube. (n.d.).]

Just like X-axis scaling, Z-axis scaling creates instances of an application that are functionally identical.
What sets it apart is how it deals with bias: instead of a load balancer that just evenly distributes tasks, data
partitioning splits the data so each server is responsible for a subset of it, with an additional component that routes
requests to the server responsible for them.
This is often done based on some parameter of the request, or the type of user from which the request comes.

![](embed:rs_scale_z)

This technique is often used for scaling databases, where data is sharded based on a property - such as in the
example -, but can also be applied to applications.
Z and X axis scaling can be combined by deploying multiple replicas of each partition, allowing for scaling highly used
partitions.

#### Y-axis scaling: decomposition

[The Scale Cube. (n.d.).]

Last but not least is functional decomposition, the basis of microservices.
Instead of trying to scale a single application, we split it into a set of separate services, with each one responsible
for a smaller, more closely related set of functions.
This can generally be done based on *verbs*, meaning a service implements a single use case, such as "checkout", or on
*nouns*, where a service is responsible for all operations related to a certain entity, such as "customer".

![](embed:rs_scale_y_fs)
![](embed:rs_scale_y_qs)

Decomposition allows us to apply X and Z scaling to the exact part of the system that needs them, without effecting
other parts of it.
It also reduces the failure rate of system, as when one service fails the rest can keep going - assuming sufficient
decoupling.
In the example, if the `QuickSearch Service` fails, we can always fall back on the `FullSearch Service`.
If the `Image Service` fails, we can still serve the vehicle's data, just without the image.

#### To Chose a Method

These scaling methods aren't exclusive.
Each is suitable for some problems, while not suitable for others, and they can be combined to solve more complex
scaling issues.
Thus, we will have to make decisions on a case-by-case basis.
