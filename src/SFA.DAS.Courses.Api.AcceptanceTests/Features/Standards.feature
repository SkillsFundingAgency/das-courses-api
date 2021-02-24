Feature: Standards
	As a Courses API consumer
    I want to retrieve standards
    So that I can use them in my own application

    Scenario: Get list of active and available standards
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=ActiveAvailable
    Then an http status code of 200 is returned
    And all valid standards are returned

    Scenario: Get list of standards by keyword that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title       | level | sector                        | version | status                |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |
    | Head Brewer | 2     | Engineering and manufacturing | 1.3     | Approved for delivery |

    Scenario: Get list of standards by keyword with none filter
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=None&keyword=beer
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                      | level | sector                        | version | status                 |
    | Brewer                                     | 1     | Engineering and manufacturing | 1.0     | Approved for delivery  |
    | Assistant Brewer - Proposal in development | 1     | Engineering and manufacturing | 1.1     | Proposal in development|
    | Assistant Brewer - Withdrawn               | 1     | Engineering and manufacturing | 1.0     | Withdrawn              |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.2     | Retired                |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.1     | Retired                |

    Scenario: Get list of standards by keyword
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                      | level | sector                        | version | status                 |
    | Brewer                                     | 1     | Engineering and manufacturing | 1.0     | Approved for delivery  |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    
    Scenario: Get list of standards by levels that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?levels=1&levels=7&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title   | level | sector                        | version | status                |
    | Brewer  | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |
    | Dentist | 7     | Construction                  | 1.0     | Approved for delivery |

    Scenario: Get list of standards by sectors that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?routeIds=B30D7750-9ADF-41BA-94BD-E4584128EC76&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                           | level | sector       | version | status                |
    | Dentist                                         | 7     | Construction | 1.0     | Approved for delivery |
    | Senior / head of facilities management (degree) | 6     | Construction | 1.0     | Approved for delivery |

    Scenario: Get list of standards by keyword and levels that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=beer&levels=1&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title       | level | sector                        | version | status                |
    | Brewer      | 1     | Engineering and manufacturing | 1.0     | Approved for delivery |

    Scenario: Get list of standards by keyword sorted by relevance that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=sortorder&orderBy=score&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                            | level | sector              | version | status                |
    | Junior animator SortOrder        | 4     | Creative and design | 1.0     | Approved for delivery |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.1     | Approved for delivery |
    | Camera prep technician           | 3     | Creative and design | 1.0     | Approved for delivery |

    Scenario: Get list of standards by keyword sorted by name that are active and available
	Given I have an http client
    When I GET the following url: /api/courses/standards?keyword=sortorder&orderBy=title&filter=ActiveAvailable
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                            | level | sector              | version | status                |
    | Camera prep technician           | 3     | Creative and design | 1.0     | Approved for delivery |
    | Junior animator SortOrder        | 4     | Creative and design | 1.0     | Approved for delivery |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.1     | Approved for delivery |

    Scenario: Get list of active standards not restricted by start date
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=Active
    Then an http status code of 200 is returned
    Then all valid and invalid standards are returned

    Scenario: Get list of all standards
	Given I have an http client
    When I GET the following url: /api/courses/standards?Filter=None
    Then an http status code of 200 is returned
    And all standards are returned

    Scenario: Get list the default list of standards
	Given I have an http client
    When I GET the following url: /api/courses/standards
    Then an http status code of 200 is returned
    And all valid standards are returned

    Scenario: Get list of not yet approved standards
	Given I have an http client
    When I GET the following url: /api/courses/standards?filter=NotYetApproved
    Then an http status code of 200 is returned
    And all not yet approved standards are returned

    Scenario: Get latest version of a standard by Lars Code
	Given I have an http client
    When I GET the following url: /api/courses/standards/1
    Then an http status code of 200 is returned
    And the following standard is returned
    | title                            | level | sector                        | version | status                |
    | Head Brewer                      | 2     | Engineering and manufacturing | 1.3     | Approved for delivery |
    
    Scenario: : Get a specific version of a standard by StandardUId
    Given I have an http client
    When I GET the following url: /api/courses/standards/ST005_1.0
    Then an http status code of 200 is returned
    And the following standard is returned
    | title                            | level | sector              | version | status  |
    | Photographic assistant SortOrder | 3     | Creative and design | 1.0     | Retired |

    Scenario: : Get a all versions of a standard by IFateReferenceNumber
    Given I have an http client
    When I GET the following url: /api/courses/standards/versions/ST001
    Then an http status code of 200 is returned
    And the following valid standards are returned
    | title                                      | level | sector                        | version | status                 |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.3     | Approved for delivery  |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.2     | Retired                |
    | Head Brewer                                | 2     | Engineering and manufacturing | 1.1     | Retired                |