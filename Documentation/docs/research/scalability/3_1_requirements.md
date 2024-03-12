## What requirements does Kwetter have that a solution has to account for?

A few requirements are dictated by external factors, such as Fontys and financial limitations:

1. The solution must utilize a microservice architecture to fulfill the learning goals.
2. All technology choices must be either free-to-use or have a free-for-students/hobbyists version available to avoid
   incurring costs.
3. Whenever possible, existing tools or technologies should be used instead of manual implementation.

### Development Requirements

Given the constraints of a small team (one person), the solution should consider the following requirements:

1. Prefer solutions that can be used with C# or Python-based applications, as these are the primary programming
   languages within the team's skill set.
2. Give preference to technologies that have good existing community support, as it facilitates learning,
   problem-solving, and access to resources.

### Measurable Requirements

We aim to achieve the following measurable requirements:

1. Scalability: The solution should be able to handle a large number of users and posts without experiencing performance
   degradation or downtime. This requires designing microservices for horizontal scaling and ensuring effective load
   balancing. We aim to support at least 10,000 concurrent users and handle a minimum of 100 posts per second in the
   development build.
2. Reliability: The solution should be highly reliable and available at all times, with minimal downtime or maintenance
   windows. This requires implementing redundant microservices and failover mechanisms to ensure high availability. We
   aim to achieve an uptime of 99.9% over a one-year period.
3. Performance: The solution should deliver fast response times and low latency to provide a seamless user experience.
   This requires optimizing the performance of each microservice and designing them to be efficient and lightweight. We
   aim to achieve an average response time of less than 200 milliseconds for 95% of read requests.
4. Real-time Updates: New kweets should be available to the user within 5 seconds of submitting them. This ensures that
   users can see their own kweets without delay. Furthermore, all users should be able to view the new kweets within a
   minute of their submission. This requires implementing efficient real-time data synchronization mechanisms between
   microservices and minimizing the propagation delay of new kweets across the system.
5. Maintainability: The solution should be easy to maintain and update, with minimal downtime or disruption to users.
   This requires designing the microservices to be modular and loosely coupled. We aim to allow updates to individual
   microservices without affecting the availability of the entire system, with a maximum maintenance window of one hour
   per month.
6. Extensibility: The solution should be extensible and easily customizable, enabling the addition of new features or
   modification of existing ones without significant changes to the underlying architecture. This requires designing the
   microservices to be flexible and adaptable, with well-defined APIs.
7. Security: The solution should be highly secure to protect user data and prevent unauthorized access. This includes
   implementing secure user authentication, data encryption, and access control measures. We aim to achieve compliance
   with industry-standard security frameworks (e.g., ISO 27001) and successfully pass a third-party security audit.
8. Monitoring and Logging: The solution should be capable of monitoring and logging key performance metrics and events
   to quickly identify and resolve issues. This requires implementing a robust monitoring and logging system. We aim to
   capture and analyze performance metrics in real-time with a maximum delay of one minute and retain logs for at least
   six months.