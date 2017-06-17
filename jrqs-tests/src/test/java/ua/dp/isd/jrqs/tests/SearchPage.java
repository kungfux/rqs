package ua.dp.isd.jrqs.tests;

import java.util.concurrent.TimeUnit;
import org.junit.After;
import static org.junit.Assert.assertArrayEquals;
import org.junit.Before;
import org.junit.Test;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.ie.InternetExplorerDriver;
import static org.openqa.selenium.support.ui.ExpectedConditions.titleIs;
import org.openqa.selenium.support.ui.WebDriverWait;

public class SearchPage {

    private final String Url = "http://localhost/jrqs";
    private final String Title = "jRQS - Home";

    private WebDriver driver = new InternetExplorerDriver();
    private final WebDriverWait wait = new WebDriverWait(driver, 10);
    private final SearchPageHelper searchPage = new SearchPageHelper(driver);
    

    @Before
    public void start() {
        driver.manage().timeouts().implicitlyWait(10, TimeUnit.SECONDS);
        driver.get(Url);
        wait.until(titleIs(Title));
    }

    @Test
    public void TestSeacrhBySingleFRID() {
        String searchText = "FR1";
        String[] expected = searchText.split(",");
        assertArrayEquals(expected, searchPage.doSearchBy(searchText, 2));
    }
    
    @After
    public void stop() {
        driver.quit();
        driver = null;
    }
}
