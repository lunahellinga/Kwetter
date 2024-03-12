## Requirements

**What requirements does Kwetter have that a solution has to account for?**

Given the limited budget of the project, we'll consider practical and cost-effective solutions. In addition, any cloud
hosting solutions must a free tier or provide enough free credits to be considered.

### Data Storage

Text Kweets: Kwetter needs a data storage solution capable of efficiently storing and retrieving text-based Kweets. A
relational database or a NoSQL document-oriented database can be suitable for this purpose. Relational databases like
MySQL or PostgreSQL can provide strong data integrity and querying capabilities. Alternatively, document-oriented
databases like MongoDB or CouchDB can offer flexibility in handling unstructured data.

Profiles: User profiles with additional details require a storage solution that can handle structured data. A
relational database can be used to store profile information such as username, bio, profile picture, and additional
user details.

Relationships: To handle the follow relationships between users, a graph database like Neo4j or a relational database
with appropriate schema design can be utilized.

### Data Integrity

ACID Compliance: Kwetter needs a database system that supports ACID properties to ensure data integrity. This is
especially crucial for critical operations such as posting Kweets, updating profiles, handling likes and rekweets, and
managing follower relationships. Relational databases are well-suited for providing ACID guarantees, but careful
database design and transaction management are necessary.

### Data Retention

Kweet History: Kwetter requires a mechanism to retain the history of Kweets to facilitate features such as viewing
past Kweets, analyzing trends, and generating timelines. Depending on the projected data volume, an appropriate
retention policy needs to be established. This can involve periodically archiving older Kweets to offline storage or
leveraging data partitioning and purging mechanisms.

Profile History: Similar to Kweets, user profiles may also require historical data retention. This can be achieved by
maintaining a versioning mechanism in the database or implementing an audit log to track profile changes over time.

### Scalability

As Kwetter aims to mimic a popular social media platform like Twitter, scalability considerations are essential. The
selected database solution should provide horizontal scalability to handle a growing user base and increased data
volume. This can be achieved through distributed database systems like CockroachDB, TiDB, or YugabyteDB, which offer
automatic sharding and replication.

### Backup and Disaster Recovery

Kwetter should implement regular database backups and establish a disaster recovery plan. This involves scheduling
backups of the database and associated data, ensuring backup integrity, and defining procedures for restoring the
system in case of data loss or system failures.

### Security and Privacy

To protect user data, Kwetter needs to implement appropriate security measures, such as encryption of sensitive
information, secure user authentication and authorization mechanisms, and adherence to relevant data protection
regulations (e.g., GDPR).

### Cost

Considering the limited budget and scope, it's important to strike a balance between the functionality and the cost of
infrastructure and database solutions. Open-source databases can provide cost-effective options, while cloud-based
database services like AWS RDS, Azure SQL Database, or Google Cloud SQL can offer scalability and managed services
within a budget.

### Caching

To improve the performance of read operations in Kwetter, caching can be implemented. Caching involves storing
frequently accessed data in memory for fast retrieval. [What is Caching and How it Works | AWS. (n.d.).]

1. Caching Layer:
    - Implement a caching layer using technologies like MongoDB, Memcached or Redis to cache frequently accessed data,
      such as user profiles, trending tags, and popular kweets.
    - Configure the caching layer to store the data in memory, allowing for fast reads without the need to query the
      underlying database.
    - Utilize appropriate cache eviction policies to manage the cache size and prioritize frequently accessed data.

2. Cache Invalidation:
    - Implement cache invalidation mechanisms to ensure that cached data remains consistent with the underlying
      database.
    - When data is updated (e.g., when a user posts a new kweet or someone likes a kweet), invalidate the corresponding
      cache entries to ensure that subsequent reads retrieve the most up-to-date information.
    - Consider using cache invalidation strategies like "write-through" or "write-behind" to update the cache
      synchronously or asynchronously with database updates.

3. Caching Strategy:
    - Identify the portions of data that are read-intensive and suitable for caching, such as user profiles, popular
      kweets, and trending tags.
    - Determine the appropriate caching granularity based on access patterns, ensuring that cached data remains relevant
      and useful.
    - Consider using techniques like "cache aside" or "read-through" to fetch data from the cache if available or
      retrieve it from the database and cache it for subsequent reads.