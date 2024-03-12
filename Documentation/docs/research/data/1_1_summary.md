## Summary

In this research we set out to find suitable solutions to store and handle data in Kwetter.

Kwetter requires a database solution that can efficiently store and retrieve text-based Kweets, user profiles, and
relationships. It needs ACID compliance for data integrity and mechanisms for data retention. Scalability, backup, and
disaster recovery are important considerations, along with security measures and cost-effectiveness.

Based on performance testing, MongoDB is suitable for small-scale unstructured data, while Postgres performs well with
large datasets and sorting operations. Neo4j is more suitable for complex relationship analytics, and Yugabyte's
performance tests had different conditions.

By considering these requirements and test results, Kwetter can make informed decisions about the appropriate database
systems to use for specific functionalities, ensuring a balance between functionality and cost.



