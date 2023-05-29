
This application allows users to obtain country data from https://restcountries.com/v3.1/all and provides additional quality of life features, like
filtering, sorting and paging.

Here are a few examples of how users can use the provided endpoint:

Example 1: Retrieve all countries
Request:
```
GET /countries
```
Response:
```
HTTP 200 OK
[
  {
    "name": "Country 1",
    "population": 1000000,
    ...
  },
  {
    "name": "Country 2",
    "population": 5000000,
    ...
  },
  ...
]
```

Example 2: Retrieve countries with a specific name
Request:
```
GET /countries?name=Canada
```
Response:
```
HTTP 200 OK
[
  {
    "name": "Canada",
    "population": 37589262,
    ...
  }
]
```

Example 3: Retrieve countries with a minimum population and sort by population in ascending order
Request:
```
GET /countries?population=10000000&sort=population&limit=10
```
Response:
```
HTTP 200 OK
[
  {
    "name": "Country 1",
    "population": 10000000,
    ...
  },
  {
    "name": "Country 2",
    "population": 12000000,
    ...
  },
  ...
]
```



These examples demonstrate how the endpoint can be used to retrieve country data based on different criteria such as name, population, sorting, and limiting the number of results.