### Architecture

Kwetter's architecture consists of five major systems, each serving specific functionalities.

![](embed:SystemLandscape)

#### Access System:
- This system encompasses user-facing services, including the API gateway, web application, and Keycloak
   authentication provider.
- The API gateway is responsible for managing all user calls to the backend services, acting as a single entry point
for user interactions.

![](embed:AccessServices)

#### Posting System:
- The Posting System handles the functionalities related to posting Kweets.
- The central component is the Kweeter service, which acts as an orchestrator for posting Kweets.
- The Kweet Service stores Kweets and their metadata, such as timestamps, tags and statistical data.
- The User Post Service is responsible for tracking who posts what.
- The Content Service manages the storage and retrieval of Kweet content, such as text and media attachments.
- The Mention Service handles the processing and tagging of user mentions in Kweets.

![](embed:PostingServices)

#### Reading System:
- The Reading System focuses on serving Kweets and related data to users.
- The Kweader utilizes a cache to efficiently serve recent and high-traffic Kweets, improving response times.
- It fetches data from various services in the Posting System to assemble and present the Kweets.
- The Reading System also includes the Timeline Service, which manages the timeline functionality, displaying Kweets
from followed users, and the Search Service, which enables users to search for Kweets based on tags or users.

![](embed:ReaderServices)

#### Trend System:
- The Trend System is responsible for identifying and presenting trends based on tags used in Kweets.
- The Trend Service analyzes Kweets and determines the popular and trending topics.
- The Trend Analyzer processes Kweets to identify relevant tags and trends.
- The Trend Data Service stores and manages trend data, allowing for efficient retrieval and presentation.

![](embed:TrendServices)

#### Interaction System:
- The Interaction System handles various interactions related to Kweets and users.
- The Reply Service manages replies to Kweets, enabling users to engage in conversations.
- The Like Service tracks and manages the likes given to Kweets by users.
- The Share Service allows users to share Kweets via the rekweet functionality.
- The Follow Service enables users to follow other users, ensuring their Kweets appear in the follower's timeline.

![](embed:InteractionServices)


### Reality

![](embed:Kwetter)
