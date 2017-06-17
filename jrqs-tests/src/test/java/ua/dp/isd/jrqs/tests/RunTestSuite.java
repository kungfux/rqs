package ua.dp.isd.jrqs.tests;

import junit.framework.TestCase;

import org.junit.AfterClass;
import org.junit.BeforeClass;
import org.junit.runner.RunWith;
import org.junit.runners.Suite;
import org.junit.runners.Suite.SuiteClasses;

@RunWith(Suite.class)
@SuiteClasses({SearchByRowIdTests.class,SearchByFRIDTests.class,SearchByTmsTaskTests.class,SearchByTextTests.class})
public class RunTestSuite extends TestCase {

    private static WebHelper wh;

    @BeforeClass
    public static void start() {
        wh = new WebHelper();
    }

    @AfterClass
    public static void stop() {
        wh.getDriverInstance().quit();
    }
}
