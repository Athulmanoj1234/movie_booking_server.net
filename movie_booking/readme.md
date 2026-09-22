Rate Limiting 

  It is a technique to limits the quantity or frequency of client requests to prevent overload, maintain stability, and ensure fair resource distribution.
        -> Reduces the risk of resource abuse and denial-of-service (DoS) attacks, improving performance, reliability, and security.
        -> It is used in web servers, APIs, network traffic management, and database access.

   It can be used in the different components of the server
        -> API Rate Limiting :- APIs commonly employ rate limitation to control the volume of client requests, ensure fair access to resources, and prevent abuse.
        -> Web Server Rate Limiting :- Web servers employ rate limitation as a defense against denial-of-service attacks and to prevent server overload.
        -> Database Rate limitation :- To keep the database server from experiencing undue strain and to preserve database performance, rate limitation is applied to database queries. For instance, to avoid resource exhaustion and guarantee seamless functioning, an e-commerce website can restrict the quantity of database queries per user.
        Login Rate restriction:- To stop password guessing and brute-force assaults, login systems employ rate restriction. Systems can prevent unwanted access by restricting the quantity of login attempts made by each person or IP address.


    Different types of rate limiting
        -> IP-based Rate Limiting :- it limits number of web or api requests from a single ip address within a set time period.
            * Even though the data sent back is identical, the server still has to do heavy lifting for every single request:
            * If a server can only handle 1,000 requests per second, and a single bot dumps 50,000 requests onto it in one second, the server's CPU and memory hit 100%. The server crashes, and real, legitimate users can no longer access the website. This is a Denial of Service (DoS) attack.
            * over exploitation of data or datascraping is limited.
            Request 1: ://mysite.com (Returns Product A)Request 2: ://mysite.com (Returns Product B)Request 3: ://mysite.com (Returns Product C)...Request 50,000: ://mysite.com (Returns Product Z)

            Advantages
                -> This approach is widely used due to its simplicity and effectiveness in basic traffic control.
                -> simple to implement at both network and application levels
                -> Helps block excessive traffic from a single source
            Disadvantages
                -> Can be bypassed using VPNs, proxies, or botnets
                -> (very important)May block legitimate users sharing the same IP (e.g., corporate networks)

        
        -> server-based rate limiting :- It limits number of requests a server can handle within a specific timeperiod to prevent overload and maintain performance.
            for eg - Example: A music streaming service allows only 100 requests per second per server to ensure the system remains fast and responsive during peak usage.

            Advantages
                -> This approach helps maintain system stability by controlling traffic at the server level.
                -> Protects servers from being overwhelmed during high traffic
                -> Ensures fair resource usage so no single user degrades performance
            Disadvantages
                -> However, it may not be fully effective in distributed environments.
                -> Can be bypassed if requests are spread across multiple servers
                -> Legitimate users may face delays if limits are too strict or traffic is high

        -> Geographical ratelimiting :- more to be explored......

        Working
            The number of queries a user or system can make to a service in a predetermined period of time can be managed by rate limitation. A service might permit 100 requests per minute, for instance. Any additional requests will be blocked or slowed down by the system until the time window is reset once that limit is reached.