

This was an interesting coding exercise. The technical areas required (Web front end, web API, OAuth) are not really my areas
of expertise, so I spent a large portion of my time learning how to do things and unfortunately I've run out of time. 

I've managed to get the stated functional requirements working. If you run the solution, it should bring up a web page showing 
the aggregated tweets from the specified twitter accounts, plus the calculated fields. The core functionality is covered by
unit tests.

There is a fake API, which if launched, can be substituted for the real twitter REST endpoint by going to the PayByPhoneCodingExercise
project and editing the web.config. There are config keys for the REST endpoint and for the app tokens (it uses Application-only
Authentication, so it doesn't need user tokens).

There are a number of areas I would improve if I had more time:
	* The calls to get tweets for different users should be parallelized, this would speed up execution time. 
	* The loading of tweet JSON by the front end should be done asynchronously, e.g. via and AJAX call from the client
		 * as it is now, the web page takes quite a long time to come up as its fetching all data syncrhonously
	* I would like to have added unit tests for the controllers
	* I would like to have added unit tests for the Fake API

Regards,

Andrew